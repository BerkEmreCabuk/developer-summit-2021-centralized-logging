using DeveloperSummit.Core.Infrastructure.RabbitMq;
using DeveloperSummit.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace DeveloperSummit.Core.Extensions
{
    public static class RabbitMqExtension
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, RabbitMqConfigModel rabbitMqConfig)
        {
            services.AddCap(x =>
            {
                x.UseInMemoryStorage();
                x.UseRabbitMQ(opt =>
                {
                    opt.HostName = rabbitMqConfig.RabbitMqHostname;
                    opt.Password = rabbitMqConfig.RabbitMqPassword;
                    opt.UserName = rabbitMqConfig.RabbitMqUsername;
                    opt.Port = 5672;
                    opt.ExchangeName = "developer.summit";
                });
                x.ConsumerThreadCount = 2;
                x.FailedRetryCount = 5;

            });
            services.AddScoped<IRabbitMqService, RabbitMqService>();
            return services;
        }
    }
}
