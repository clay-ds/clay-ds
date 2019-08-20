using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((context, config) => config
                    .ReadFrom.Configuration(context.Configuration)
                    .WriteTo.Console()
                    .WriteTo.Seq(context.Configuration["SeqServer"])
                    .Enrich.FromLogContext());
    }
}
