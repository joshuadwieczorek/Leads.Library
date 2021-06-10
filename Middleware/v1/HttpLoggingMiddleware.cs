using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Leads.Library.Sdks.v1;
using Leads.Domain.Models.v1;
using Microsoft.Extensions.Configuration;
using Leads.Domain;
using Microsoft.AspNetCore.Http.Extensions;

namespace Leads.Library.Middleware.v1
{
    public class HttpLoggingMiddleware
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly RequestDelegate _next;



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        public HttpLoggingMiddleware(
              ILoggerFactory loggerFactory
            , IConfiguration configuration
            , RequestDelegate next)
        {
            _logger = loggerFactory.CreateLogger<HttpLoggingMiddleware>();
            _configuration = configuration;
            _next = next;
        }


        /// <summary>
        /// Log request to file.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var serviceUrl = _configuration["ServiceUrl"];
            using HttpLogSdk httpLogHelper = new HttpLogSdk(_logger, serviceUrl);
            context.Request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            var requestBody = Encoding.UTF8.GetString(buffer);
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            var headerString = new StringBuilder();
            foreach (var header in context.Request.Headers)
                headerString.AppendLine($"{header.Key}:{header.Value}");

            var log = new HttpLogModel
            {
                HttpMethod = context.Request.Method.ToLower() switch
                {
                    "post" => Enums.HttpMethod.POST,
                    "get" => Enums.HttpMethod.GET,
                    "put" => Enums.HttpMethod.PUT,
                    "patch" => Enums.HttpMethod.PATCH,
                    "delete" => Enums.HttpMethod.DELETE,
                    _ => Enums.HttpMethod.UNKNOWN
                },
                StartTime = DateTime.Now,
                Url = context?.Request?.GetDisplayUrl(),
                IpAddress = context.Request.Headers.ContainsKey("X-Forwarded-For")
                    ? context?.Request.Headers["X-Forwarded-For"][0]
                    : context?.Connection?.RemoteIpAddress?.ToString()
            };

            log.SetRequestHeaders(headerString.ToString());
            log.SetRequestBody(requestBody);
            log = await httpLogHelper.CreateLog(log);

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            log.EndTime = DateTime.Now;
            log.StatusCode = context.Response.StatusCode;
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBodyString = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            log.SetResponseBody(responseBodyString);
            await httpLogHelper.UpdateLog(log);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}