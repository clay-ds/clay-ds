using IdentityServer.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var clientOptions = _configuration.GetSection("Clients").Get<ClientOptions>();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(IdentityResources.GetAll())
                .AddInMemoryApiResources(ApiResources.GetAll())
                .AddInMemoryClients(Clients.GetAll(clientOptions))
                .AddTestUsers(Users.GetUsers());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseIdentityServer();
        }
    }
}
