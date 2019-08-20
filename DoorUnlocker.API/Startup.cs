using AutoMapper;
using DoorUnlocker.API.Application.Services.Authorization;
using DoorUnlocker.API.Infrastructure.Configuration;
using DoorUnlocker.API.Infrastructure.Filters;
using DoorUnlocker.API.Infrastructure.Middleware;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DoorUnlocker.API
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
            services
                .AddMvc(options =>
                {
                    options.Filters.Add(new AuthorizeFilter(AuthorizationPolicies.DefaultPolicy));
                    options.Filters.Add(new ValidationFilter());
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                })
                .AddFluentValidation(o => o.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddAutoMapper(typeof(Startup).Assembly);

            ConfigureAuthentication(services);
            ConfigureAuthorization(services);
            
            services.RegisterDataAccess(_configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.EnsureDatabase();

            app.UseMiddleware<LoggingMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();
            app.UseMvc();
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            var idsOptions = _configuration.GetSection("IdentityServer").Get<IdentityServerOptions>();

            services.AddAuthentication("Bearer")
                .AddJwtBearer(opt =>
                {
                    opt.Authority = idsOptions.Authority;
                    opt.RequireHttpsMetadata = idsOptions.RequireHttpsMetadata;

                    opt.Audience = idsOptions.Audience;

                    opt.TokenValidationParameters.ValidateIssuer = false;
                });
        }

        private void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(AuthorizationPolicies.ConfigureAll);

            services.AddScoped<IAuthorizationHandler, EmployeeDoorAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, AdminDoorAuthorizationHandler>();
        }
    }
}