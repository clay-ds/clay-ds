using System.Collections.Generic;
using IdentityServer4.Models;

namespace IdentityServer.Core
{
    public static class IdentityResources
    {
        public static IEnumerable<IdentityResource> GetAll()
        {
            return new IdentityResource[]
            {
                new IdentityServer4.Models.IdentityResources.OpenId(),
                new IdentityServer4.Models.IdentityResources.Profile()
            };
        }
    }
}
