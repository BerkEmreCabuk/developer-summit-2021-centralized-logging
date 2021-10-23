using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeveloperSummit.Core.Helper;
using DeveloperSummit.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.RabbitMQ;

namespace DeveloperSummit.CargoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = LoggingHelper.CustomLoggerConfiguration(new CustomLoggerConfigurationModel("CargoApi", "2"));
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseKestrel()
                    .UseUrls("http://localhost:5101")
                    .ConfigureLogging(x => x.ClearProviders())
                    .UseSerilog();
                });
    }
}
