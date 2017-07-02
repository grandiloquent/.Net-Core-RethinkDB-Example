using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EverStore.Libraries.Extensions
{
    public static class ApplicationBuilderExtensions
    {
  
        // Configure XSRF middleware, 
        // This pattern is for SPA style applications where XSRF token is added on Index page 
        // load and passed back token on every subsequent async request            
        public static IApplicationBuilder UseXsrf(this IApplicationBuilder app)
        {
            var antiforgery = app.ApplicationServices.GetRequiredService<IAntiforgery>();

            app.Use(async (context, next) =>
            {
                if (string.Equals(context.Request.Path.Value, "/", StringComparison.OrdinalIgnoreCase))
                {
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                        new CookieOptions() {HttpOnly = false});
                }
                await next.Invoke();
            });
            return app;
        }
    }

    public static class ServiceCollectionExtensions
    {
//        public static IServiceCollection AddSslCertificate(this IServiceCollection services, IHostingEnvironment hostingEnv)
//        {
//            var cert = new X509Certificate2(Path.Combine(hostingEnv.ContentRootPath, "extra", "cert.pfx"), "game123");
//
//            services.Configure<KestrelServerOptions>(options =>
//            {
//                options.UseHttps(cert);
//
//            });
//
//            return services;
//        }

    }
}