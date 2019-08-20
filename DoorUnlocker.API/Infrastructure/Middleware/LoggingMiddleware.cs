using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace DoorUnlocker.API.Infrastructure.Middleware
{
    public class LoggingMiddleware
    {
        private static readonly ILogger Logger = Log.ForContext<LoggingMiddleware>();

        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Logger.Information("HTTP Request: {Method} {Url}",
                context.Request.Method,
                context.Request.Path);

            var timer = Stopwatch.StartNew();

            await _next.Invoke(context);

            timer.Stop();

            Logger.Information("HTTP response: Status {StatusCode} after {Elapsed}",
                context.Response.StatusCode,
                timer.Elapsed);
        }
    }
}