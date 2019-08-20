using System.Collections.Generic;
using System.Security.Claims;

namespace DoorUnlocker.UnitTests.Application.Services.Authorization
{
    public class ClaimsPrincipalBuilder
    {
        private List<Claim> _claims = new List<Claim>();

        public ClaimsPrincipalBuilder WithUserId(int id)
        {
            return WithClaim(ClaimTypes.NameIdentifier, id.ToString());
        }

        public ClaimsPrincipalBuilder WithClaim(string type, string value)
        {
            _claims.Add(new Claim(type, value));

            return this;
        }

        public ClaimsPrincipal Build()
        {
            var identity = new ClaimsIdentity(_claims);
            return new ClaimsPrincipal(identity);
        }
    }
}