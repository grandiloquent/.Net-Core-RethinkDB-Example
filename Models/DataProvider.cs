using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EverStore.Libraries;
using Newtonsoft.Json.Linq;
using RethinkDb.Driver;
using RethinkDb.Driver.Ast;
using RethinkDb.Driver.Model;
using RethinkDb.Driver.Net;
using static RethinkDb.Driver.RethinkDB;

namespace EverStore.Models
{
    /*
    
    r.db('psycho').table('articles').orderBy({index:r.desc('createAt')}).skip(9).limit(10).pluck('id', 'title', 'image')
    r.dbDrop('psycho')
    r.dbList()
    r.db('psycho').tableList()
    */

    public class DataProvider : IDisposable
    {
        private readonly Logger _logger;

        private static DataProvider sDataProvider;


        private const string DATABASE_NAME = "psycho";

        private const string TABLE_NAME = "articles";
        private const string INDEX_CREATEAT = "createAt";
        private const string INDEX_TAGS = "tags";

        private readonly object[] _pluck = {"id", "title", "image"};

        public Connection Connection { get; private set; }
        public RethinkDB R { get; } = RethinkDB.R;

        public DataProvider()
        {
            _logger = new Logger {Namespace = "DataProvider"};
        }


        public static ToEpochTime GetTimestamp()
        {
            return RethinkDB.R.Now().ToEpochTime();
        }

        public async Task Connect()
        {
            this.Connection = R.Connection().Db(DATABASE_NAME).Connect();
            _logger.L("Connect to rethinkdb.");
            await CreateDatabase();
        }


        private bool ConnectTask()
        {
            try
            {
                this.Connection = R.Connection().Db(DATABASE_NAME).Connect();
                Console.WriteLine($"Connect " + $"to rethinkdb.");
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        private async Task CreateDatabase()
        {
            // create database
            await R.DbList().Contains(DATABASE_NAME).Do_(dbExists => R.Branch(
                dbExists,
                new
                {
                    db_created = 0
                }, R.DbCreate(DATABASE_NAME))).RunAsync(Connection);

            _logger.L($"Successfully created the database [{DATABASE_NAME}].");

            // create table
            await R.TableList().Contains(TABLE_NAME).Do_(tableExists => R.Branch(
                tableExists,
                new
                {
                    table_created = 0
                }, R.TableCreate(TABLE_NAME)
            )).RunAsync(Connection);

            _logger.L($"Successfully created the table [{TABLE_NAME}]");

            // create index
            await R.Table(TABLE_NAME).IndexList().Contains(INDEX_CREATEAT).Do_(indexExists => R.Branch(
                indexExists,
                new
                {
                    index_created = 0
                }, R.Table(TABLE_NAME).IndexCreate(INDEX_CREATEAT)
            )).RunAsync(Connection);


            _logger.L($"Successfully created the index [{INDEX_CREATEAT}]");

            await R.Table(TABLE_NAME).IndexWait(INDEX_CREATEAT).RunAsync(Connection);

            await R.Table(TABLE_NAME).IndexList().Contains(INDEX_TAGS).Do_(indexExists => R.Branch(
                indexExists,
                new
                {
                    index_created = 0
                }, R.Table(TABLE_NAME).IndexCreate(INDEX_TAGS)[new {multi = true}]
            )).RunAsync(Connection);


            await R.Table(TABLE_NAME).IndexWait(INDEX_TAGS).RunAsync(Connection);
        }


        public void Dispose()
        {
            Connection?.Dispose();
        }


        public static DataProvider GetInstance()
        {
            return sDataProvider ?? (sDataProvider = new DataProvider());
        }

        public Task<dynamic> Insert(Article article)
        {
            return R.Table(TABLE_NAME).Insert(article).RunAsync(Connection);
        }

        public async Task<List<Article>> ListArticlesByTag(string tag, int limit = 200)
        {
            JArray cursor = await R.Table(TABLE_NAME)
                .GetAll(tag)[new {index = "tags"}]
                .OrderBy(new Desc(INDEX_CREATEAT))
                .Pluck(_pluck)
                .Limit(limit)
                .RunAsync(Connection);

            return cursor.ToObject(typeof(List<Article>)) as List<Article>;
        }


        public async Task<List<Article>> ListLastArticles(int limit = 10)
        {
            JArray cursor = await R.Table(TABLE_NAME)
                .OrderBy(new Desc(INDEX_CREATEAT))
                .Pluck(_pluck)
                .Limit(limit)
                .RunAsync(Connection);


            return cursor.ToObject(typeof(List<Article>)) as List<Article>;
        }


        public async Task<List<Article>> ListSkip(int skipFactor, int limit = 10)
        {
            JArray cursor = await R.Table(TABLE_NAME)
                .OrderBy(new Desc(INDEX_CREATEAT))
                .Skip(skipFactor * limit)
                .Limit(limit)
                .Pluck(_pluck)
                .RunAsync(Connection);


            return cursor.ToObject(typeof(List<Article>)) as List<Article>;
        }
    }
}