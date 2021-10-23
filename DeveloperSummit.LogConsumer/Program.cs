using DeveloperSummit.Core.Extensions;
using DeveloperSummit.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DeveloperSummit.LogConsumer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var Configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", reloadOnChange: false, optional: false)
                .Build();

            IServiceCollection services = new ServiceCollection()
                                        .Configure<RabbitMqConfigModel>(Configuration.GetSection("RabbitMqConfig"))
                                        .Configure<ElasticSearchConfigModel>(Configuration.GetSection("ElasticSearchConfig"))
                                        .RegisterElasticContext()
                                        .AddSingleton<Consumer>();

            // entry to run app
            await services.BuildServiceProvider().GetService<Consumer>().Run(args);

        }
    }
}