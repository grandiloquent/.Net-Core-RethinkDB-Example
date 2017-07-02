using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EverStore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EverStore
{
    public class Startup
    {


        public Startup(IHostingEnvironment env)
        {
            Console.WriteLine(env.ContentRootPath);
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }


        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Set the logger level.  
            loggerFactory.AddConsole((n, l) => l >= LogLevel.Warning, true);

            // Connect to rethinkdb
            DataProvider.GetInstance().Connect().Wait();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{action=Index}",
                    new { controller = "Home" });

                routes.MapRoute("t", "t/{*tag}",
                    new { controller = "Tag", action = "Index" });

                routes.MapRoute("api", "api/json/list/{*key}",
                    new { controller = "Api", action = "Index" });


                routes.MapRoute("psycho", "psycho/{action}",
                    new { controller = "Admin" });
            });
        }
    }
}