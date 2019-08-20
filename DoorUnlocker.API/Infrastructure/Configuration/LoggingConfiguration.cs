using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace DoorUnlocker.API.Infrastructure.Configuration
{
    public static class LoggingConfiguration
    {
        public static void Configure(WebHostBuilderContext context, LoggerConfiguration config)
        {
            config
                .ReadFrom.Configuration(context.Configuration)
                .WriteTo.Console()
                .WriteTo.Seq(context.Configuration["SeqServer"])
                .Enrich.FromLogContext();
        }
    }
}