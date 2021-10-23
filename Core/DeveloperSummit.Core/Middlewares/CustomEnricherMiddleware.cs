using DeveloperSummit.Core.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperSummit.Core.Middlewares
{
    public class CustomEnricherMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomEnricherMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            int currentSeq = 0;
            if (context.Request.Headers[HeaderKeyConstants.RequestSequence].Any())
            {
                currentSeq = Convert.ToInt32(context.Request.Headers[HeaderKeyConstants.RequestSequence].FirstOrDefault()) + 1;
                context.Request.Headers[HeaderKeyConstants.RequestSequence] = currentSeq.ToString();
            }

            context.Request.EnableBuffering();
            var payload = "";
            var req = context.Request;
            using (StreamReader reader = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
            {
                payload = await reader.ReadToEndAsync();
            }
            req.Body.Position = 0;

            using (LogContext.PushProperty(LogKeyConstants.Payload, payload))
            using (LogContext.PushProperty(LogKeyConstants.QueryString, context.Request.QueryString.Value))
            using (LogContext.PushProperty(LogKeyConstants.HttpMethod, context.Request.Method.ToString()))
            using (LogContext.PushProperty(LogKeyConstants.Url, context.Request.GetDisplayUrl()))
            using (LogContext.PushProperty(LogKeyConstants.Version, context.Request.Headers[HeaderKeyConstants.Version].FirstOrDefault()))
            using (LogContext.PushProperty(LogKeyConstants.CorrelationId, context.Request.Headers[HeaderKeyConstants.CorrelationId].FirstOrDefault()))
            using (LogContext.PushProperty(LogKeyConstants.RemoteIpAddress, context.Connection.RemoteIpAddress.ToString()))
            using (LogContext.PushProperty(LogKeyConstants.RequestSequence, currentSeq.ToString()))
            {
                await _next.Invoke(context);
            }
        }
    }
}
