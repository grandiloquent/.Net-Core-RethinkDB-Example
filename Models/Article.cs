using System.Collections.Generic;
using System.Threading.Tasks;
using RethinkDb.Driver.Ast;
using Newtonsoft.Json.Linq;


namespace EverStore.Models
{
    public class Article
    {
        public string Id;
        public string Title;
        public string Content;
        public string Image;
        public ToEpochTime CreateAt;
        public string[] Tags;
    }
}