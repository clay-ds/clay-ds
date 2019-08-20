using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture;
using DoorUnlocker.API.Application.Services.Authorization;
using Microsoft.AspNetCore.Authorization;
using Xunit;

namespace DoorUnlocker.UnitTests.Application.Services.Authorization
{
    public class AdminDoorAuthorizationHandlerTests
    {
        private readonly Fixture _fixture = new Fixture();
        
        private AdminDoorAuthorizationHandler _handler;
        
        public AdminDoorAuthorizationHandlerTests()
        {
            _handler = new AdminDoorAuthorizationHandler();
        }

        [Fact]
        public async Task Handle_Succeeds_If_Admin()
        {
            var requirement = _fixture.Create<DoorAuthorizationRequirement>();
            
            var principal = new ClaimsPrincipalBuilder()
                .WithUserId(1)
                .WithClaim(ClaimTypes.Role, "admin")
                .Build();
            
            var ctx = new AuthorizationHandlerContext(new[] { requirement }, principal, null);

            await _handler.HandleAsync(ctx);
            
            Assert.True(ctx.HasSucceeded);
        }

        [Fact]
        public async Task Handle_DoesNot_Succeed_If_Not_Admin()
        {
            var requirement = _fixture.Create<DoorAuthorizationRequirement>();
            
            var principal = new ClaimsPrincipalBuilder()
                .WithUserId(1)
                .WithClaim(ClaimTypes.Role, "employee")
                .Build();
            
            var ctx = new AuthorizationHandlerContext(new[] { requirement }, principal, null);

            await _handler.HandleAsync(ctx);
            
            Assert.False(ctx.HasSucceeded);
            Assert.False(ctx.HasFailed);
        }
    }
}