using DeveloperSummit.Core.Constants;
using DeveloperSummit.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperSummit.Core.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            if(!context.Request.Headers.TryGetValue(HeaderKeyConstants.CorrelationId, out StringValues correlationId))
            {
                correlationId = string.IsNullOrEmpty(context.TraceIdentifier) ? Guid.NewGuid().ToString() : context.TraceIdentifier;
                context.Request.Headers.Add(HeaderKeyConstants.CorrelationId, correlationId);
            }
            if (!context.Response.Headers.Any(x => x.Key == HeaderKeyConstants.CorrelationId))
            {
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.TryAdd(HeaderKeyConstants.CorrelationId, new[] { context.TraceIdentifier });
                    return Task.CompletedTask;
                });
            }
            await _next(context);
        }
    }
}
