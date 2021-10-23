using System;
using DeveloperSummit.Core.Helper;
using DeveloperSummit.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.RabbitMQ;

namespace DeveloperSummit.ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = LoggingHelper.CustomLoggerConfiguration(new CustomLoggerConfigurationModel("ApiGateway", "1"));
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
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel();
                    webBuilder.UseUrls("http://localhost:5100");
                    webBuilder.ConfigureAppConfiguration((host, config) =>
                    {
                        config.AddJsonFile("ocelot.json");
                    });
                    webBuilder.UseSerilog();
                });
    }
}
