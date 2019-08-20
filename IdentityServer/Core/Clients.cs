using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer.Core
{
    public static class Clients
    {
        public static IEnumerable<Client> GetAll(ClientOptions options)
        {
            return new []
            {
                new Client
                {
                    ClientId = "doorsApiClient",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret(options.ApiClientSecret.Sha256()) },
                    
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        ApiResources.Doors
                    }
                },
            };
        }
    }
}
