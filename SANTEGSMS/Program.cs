using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SANTEGSMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
                 Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("Logs\\log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()            
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseKestrel(options =>
                    // {
                    //     //options.Listen(IPAddress.Loopback, 5000);  // http:localhost:5000
                    //     //options.Listen(IPAddress.Any, 80);         // http:*:80
                    //     options.Listen(IPAddress.Loopback, 446, listenOptions =>
                    //     {
                    //         listenOptions.UseHttps("certificate.pfx", "Password123$");
                    //     });
                    // });
                    //webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    //webBuilder.UseIISIntegration();
                    //webBuilder.UseUrls("https://*:4430");
                    webBuilder.UseStartup<Startup>();

                    //webBuilder.Build();
                });
    }
}
