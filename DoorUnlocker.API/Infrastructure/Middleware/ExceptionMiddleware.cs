using System;
using System.Net;
using System.Threading.Tasks;
using DoorUnlocker.API.Infrastructure.Exceptions;
using DoorUnlocker.API.Infrastructure.Responses;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

namespace DoorUnlocker.API.Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private static readonly ILogger Logger = Log.ForContext<ExceptionMiddleware>();
        
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException ex)
            {
                Logger.Warning(ex, ex.Message);
                await BuildResponse(context, ex.Message, ex.StatusCode);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Unhandled exception : { ex.Message }");
                await BuildResponse(context, "Internal Server Error", HttpStatusCode.InternalServerError);
            }
            
        }

        private async Task BuildResponse(HttpContext context, string message, HttpStatusCode code)
        {
            if (context.Response.HasStarted)
            {
                Logger.Error("Response already started");
                return;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ExceptionResponse
            {
                Code = (int) code,
                Message = message
            }));
        }
    }
}