using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Leads.Library.Middleware.v1
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        public RequestLoggingMiddleware(
              RequestDelegate next
            , ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestLoggingMiddleware>();
        }


        /// <summary>
        /// Log request to file.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            var requestBody = Encoding.UTF8.GetString(buffer);
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            var builder = new StringBuilder(Environment.NewLine);
            builder.AppendLine($"REQUEST METHOD: {context.Request.Method}");
            builder.AppendLine($"REQUEST HEADERS ->");
            foreach (var header in context.Request.Headers)
            {
                builder.AppendLine($"{header.Key}:{header.Value}");
            }
            builder.AppendLine($"REQUEST BODY ->");
            builder.AppendLine(requestBody);
            _logger.LogInformation(builder.ToString());
            await _next(context);
        }
    }
}