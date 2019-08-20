using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer.Core
{
    public static class ApiResources
    {
        public const string Doors = "doors";

        public static IEnumerable<ApiResource> GetAll()
        {
            return new []
            {
                new ApiResource(Doors, "Doors API")
                {
                    UserClaims = 
                    {
                        JwtClaimTypes.Role,
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Profile,
                        JwtClaimTypes.NickName
                    }
                }
            };
        }
    }
}
