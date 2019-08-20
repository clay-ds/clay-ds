using System.Linq;
using DoorUnlocker.API.Application.Domain;
using DoorUnlocker.API.Application.Domain.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DoorUnlocker.API.Infrastructure.Configuration
{
    public static class DbConfiguration
    {
        public static void RegisterDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DoorsContext>(options =>
            {
                options.UseMySQL(configuration.GetConnectionString("DbConnection"))
                    .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
            });

            services.AddScoped<IDoorsRepository, DoorsRepository>();
            services.AddScoped<IOfficesRepository, OfficesRepository>();
            services.AddScoped<IPermissionsRepository, PermissionsRepository>();

            services.Configure<MongoOptions>(configuration.GetSection("MongoConnection"));
            services.AddScoped<MongoContext>();

            services.AddScoped<IEntranceLogRepository, EntranceLogRepository>();
        }
        
        // Ensures that DB is created, applies migrations and seeds with test data, of course not a production code
        public static void EnsureDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var ctx = scope.ServiceProvider.GetRequiredService<DoorsContext>())
                {
                    ctx.Database.Migrate();
                    
                    if (ctx.Offices.Count() == 0)
                    {
                        TestDbSeeder.Seed(ctx);
                    }
                }
                
                var mongoCtx = scope.ServiceProvider.GetRequiredService<MongoContext>();
                mongoCtx.Configure();
            }
        }
    }
}