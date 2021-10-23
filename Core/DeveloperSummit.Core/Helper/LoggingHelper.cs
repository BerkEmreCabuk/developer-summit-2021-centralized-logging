using DeveloperSummit.Core.Constants;
using DeveloperSummit.Core.Models;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperSummit.Core.Helper
{
    public static class LoggingHelper
    {
        public static Logger CustomLoggerConfiguration(CustomLoggerConfigurationModel model)
        {
            return new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty(LogKeyConstants.Instance, model.InstanceId)
            .Enrich.WithProperty(LogKeyConstants.DateTimeUTC, DateTime.UtcNow)
            .Enrich.WithProperty(LogKeyConstants.Application, model.Application)
            .MinimumLevel.Override("Microsoft", model.MicrosoftLogLevel)
            .MinimumLevel.Override("System", model.SystemLogLevel)
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.RabbitMQ((clientConfiguration, sinkConfiguration) =>
            {
                clientConfiguration.Username = "rabbitmq";
                clientConfiguration.Password = "rabbitmq";
                clientConfiguration.Exchange = "LoggerQueue";
                clientConfiguration.ExchangeType = "fanout";
                clientConfiguration.DeliveryMode = RabbitMQDeliveryMode.Durable;
                clientConfiguration.Port = 5672;
                clientConfiguration.VHost = "/";
                clientConfiguration.Hostnames.Add("localhost");
                sinkConfiguration.RestrictedToMinimumLevel = model.DefaultLogLevel;
                sinkConfiguration.TextFormatter = new JsonFormatter();
            })
            .CreateLogger();
        }
    }
}
