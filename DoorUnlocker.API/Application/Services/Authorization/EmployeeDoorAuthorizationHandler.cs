using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DoorUnlocker.API.Application.Domain.Repositories;
using DoorUnlocker.API.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Serilog;

namespace DoorUnlocker.API.Application.Services.Authorization
{
    public class EmployeeDoorAuthorizationHandler : AuthorizationHandler<DoorAuthorizationRequirement>
    {
        private static readonly ILogger Logger = Log.ForContext<EmployeeDoorAuthorizationHandler>();
        
        private readonly IPermissionsRepository _permissionsRepository;

        public EmployeeDoorAuthorizationHandler(IPermissionsRepository permissionsRepository)
        {
            _permissionsRepository = permissionsRepository;
        }
        
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            DoorAuthorizationRequirement requirement)
        {
            if (!context.User.HasClaim(ClaimTypes.Role, "employee"))
            {
                return;
            }
            
            var userId = context.User.GetUserId();
            if (userId == null)
            {
                return;
            }

            var permittedDoors = await _permissionsRepository.GetDoorPermissionsAsync(userId.Value);

            if (permittedDoors.Any(d => d.DoorId == requirement.DoorId))
            {
                Logger.Debug("User {UserName} has access to door {DoorId}", context.User.GetUserName(), requirement.DoorId);
                context.Succeed(requirement);
            }
        }
    }
}