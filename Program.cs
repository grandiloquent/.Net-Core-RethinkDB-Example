using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace EverStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string endpoint = "http://localhost:18081";
            string rootPath = Directory.GetCurrentDirectory();
            string environment = "Development";

            // Very simple and limited way for custom the server.
            const string settingFile = "settings.txt";
            if (settingFile.IsFile())
            {
                var lines = settingFile.ReadAllLines();

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith(nameof(endpoint)))
                    {
                        endpoint = lines[i].Slice(':')[1];
                    }
                    else if (lines[i].StartsWith(nameof(rootPath)))
                    {
                        rootPath = lines[i].Slice(':')[1];
                    }
                    else if (lines[i].StartsWith(nameof(environment)))
                    {
                        environment = lines[i].Slice(':')[1];
                    }
                }

            }

            var host = new WebHostBuilder()
                .UseKestrel(options =>
                {
                    options.AddServerHeader = false;
                })
                .UseUrls(endpoint) // Listen 
                .UseContentRoot(rootPath) // For search the views
                .UseEnvironment(environment)
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
