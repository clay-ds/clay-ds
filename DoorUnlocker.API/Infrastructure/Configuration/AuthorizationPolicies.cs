using System.Security.Claims;
using DoorUnlocker.API.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace DoorUnlocker.API.Infrastructure.Configuration
{
    public static class AuthorizationPolicies
    {
        public static void ConfigureAll(AuthorizationOptions options)
        {
            options.AddPolicy(nameof(AdminOnly), AdminOnly);
        }

        public static AuthorizationPolicy DefaultPolicy =>
            new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireAssertion(ctx => ctx.User.GetUserId() != null)
                .Build();

        public static void AdminOnly(AuthorizationPolicyBuilder policy)
        {
            policy
                .RequireAuthenticatedUser()
                .RequireClaim(ClaimTypes.Role, "admin");
        }
    }
}
