using System.Security.Claims;

namespace DoorUnlocker.API.Infrastructure.Helpers
{
    public static class IdentityHelpers
    {
        public static int? GetUserId(this ClaimsPrincipal principal)
        {
            var idString = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(idString) || !int.TryParse(idString, out var id))
            {
                return null;
            }

            return id;
        }

        public static string GetUserName(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue("name");
        }
    }
}