using DeveloperSummit.Core.Infrastructure.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;

namespace DeveloperSummit.Core.Extensions
{
    public static class HttpExtension
    {
        public static IServiceCollection AddCustomHttpClient(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient<IMainHttpService, MainHttpService>(client =>
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = new TimeSpan(0, 10, 0);
            });

            return services;
        }
    }
}
