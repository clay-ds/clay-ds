using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Test;

namespace IdentityServer.Core
{
    public static class Users
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "OneDoor",
                    Password = "password",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Role, "employee"),
                        new Claim(JwtClaimTypes.Name, "One Door Access"),
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "TwoDoors",
                    Password = "password",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Role, "employee"),
                        new Claim(JwtClaimTypes.Name, "Two Doors Access"),
                    }
                },
                new TestUser
                {
                    SubjectId = "3",
                    Username = "NoDoors",
                    Password = "password",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Role, "employee"),
                        new Claim(JwtClaimTypes.Name, "No Access"),
                    }
                },
                new TestUser
                {
                    SubjectId = "4",
                    Username = "Admin",
                    Password = "password",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Role, "admin"),
                        new Claim(JwtClaimTypes.Name, "Admin"),
                    }
                }
            };
        }
    }
}
