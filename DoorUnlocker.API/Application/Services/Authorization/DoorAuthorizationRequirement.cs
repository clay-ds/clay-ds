using Microsoft.AspNetCore.Authorization;

namespace DoorUnlocker.API.Application.Services.Authorization
{
    public class DoorAuthorizationRequirement : IAuthorizationRequirement
    {
        public int DoorId { get; set; }
    }
}