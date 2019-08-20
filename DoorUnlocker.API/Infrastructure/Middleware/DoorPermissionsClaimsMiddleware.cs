using System.Linq;
using System.Threading.Tasks;
using DoorUnlocker.API.Application.Domain.Repositories;
using Microsoft.AspNetCore.Http;

namespace DoorUnlocker.API.Infrastructure.Middleware
{
    public class DoorPermissionsClaimsMiddleware
    {
        private readonly RequestDelegate _next;

        public DoorPermissionsClaimsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IPermissionsRepository permissionsRepository)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                await Task.Delay(100);
            }

            await _next(context);
        }
    }
}
