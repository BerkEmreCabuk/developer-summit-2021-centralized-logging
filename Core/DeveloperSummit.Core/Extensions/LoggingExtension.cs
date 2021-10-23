using DeveloperSummit.Core.Constants;
using DeveloperSummit.Core.Infrastructure.ElasticSearch;
using DeveloperSummit.Core.Middlewares;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DeveloperSummit.Core.Extensions
{
    public static class LoggingExtension
    {
        public static IApplicationBuilder UseCustomLogManager(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseCustomEnricher();
            app.UseRequestPerformance();
            return app;
        }
        public static IApplicationBuilder UseCustomEnricher(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            return app.UseMiddleware<CustomEnricherMiddleware>();
        }

        public static IApplicationBuilder UseRequestPerformance(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            return app.UseMiddleware<RequestPerformanceMiddleware>();
        }

        public static IServiceCollection RegisterElasticContext(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IElasticContext, ElasticContext>();
            return services;
        }

        public static void CustomLog<T, K>(this ILogger<K> logger, CapHeader header, T data, string message, LogLevel logLevel = LogLevel.Information)
        {
            string payload = string.Empty;
            int requestSeq = 0;
            if (data != null)
                payload = JsonSerializer.Serialize(data);
            if (!string.IsNullOrEmpty(header[HeaderKeyConstants.RequestSequence]))
                requestSeq = Convert.ToInt32(header[HeaderKeyConstants.RequestSequence]) + 1;

            using (LogContext.PushProperty(LogKeyConstants.Payload, payload))
            using (LogContext.PushProperty(LogKeyConstants.QueryString, header[HeaderKeyConstants.QueryString]))
            using (LogContext.PushProperty(LogKeyConstants.Version, header[HeaderKeyConstants.Version]))
            using (LogContext.PushProperty(LogKeyConstants.CorrelationId, header[HeaderKeyConstants.CorrelationId]))
            using (LogContext.PushProperty(LogKeyConstants.RemoteIpAddress, header[HeaderKeyConstants.RemoteIpAddress]))
            using (LogContext.PushProperty(LogKeyConstants.Url, header[HeaderKeyConstants.Url]))
            using (LogContext.PushProperty(LogKeyConstants.RequestSequence, requestSeq.ToString()))
            using (LogContext.PushProperty(LogKeyConstants.RoutingKey, header["cap-msg-name"].ToString()))
            using (LogContext.PushProperty(LogKeyConstants.QueueName, header["cap-msg-group"].ToString()))
            {
                try
                {
                    switch (logLevel)
                    {
                        case LogLevel.Trace:
                            logger.LogTrace(message);
                            break;
                        case LogLevel.Debug:
                            logger.LogDebug(message);
                            break;
                        case LogLevel.Information:
                            logger.LogInformation(message);
                            break;
                        case LogLevel.Warning:
                            logger.LogWarning(message);
                            break;
                        case LogLevel.Error:
                            logger.LogError(message);
                            break;
                        case LogLevel.Critical:
                            logger.LogCritical(message);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
