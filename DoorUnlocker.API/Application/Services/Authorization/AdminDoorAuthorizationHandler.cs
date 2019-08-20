using System.Security.Claims;
using System.Threading.Tasks;
using DoorUnlocker.API.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Serilog;

namespace DoorUnlocker.API.Application.Services.Authorization
{
    public class AdminDoorAuthorizationHandler : AuthorizationHandler<DoorAuthorizationRequirement>
    {
        private static readonly ILogger Logger = Log.ForContext<AdminDoorAuthorizationHandler>();
        
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            DoorAuthorizationRequirement requirement)
        {
            if (context.User.HasClaim(ClaimTypes.Role, "admin"))
            {
                Logger.Debug("User {UserName} has access to door {DoorId} as admin", context.User.GetUserName(), requirement.DoorId);
                context.Succeed(requirement);
            }
            
            return Task.CompletedTask;
        }
    }
}