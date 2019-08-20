using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture;
using DoorUnlocker.API.Application.Domain.Models;
using DoorUnlocker.API.Application.Domain.Repositories;
using DoorUnlocker.API.Application.Services.Authorization;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Xunit;

namespace DoorUnlocker.UnitTests.Application.Services.Authorization
{
    public class EmployeeDoorAuthorizationHandlerTests
    {
        private readonly Fixture _fixture = new Fixture();

        private EmployeeDoorAuthorizationHandler _handler;

        private Mock<IPermissionsRepository> _permissionsRepostoryMock;
        
        public EmployeeDoorAuthorizationHandlerTests()
        {
            _permissionsRepostoryMock = new Mock<IPermissionsRepository>();
            
            _handler = new EmployeeDoorAuthorizationHandler(_permissionsRepostoryMock.Object);
        }

        [Fact]
        public async Task Handle_Succeeds_If_Door_Is_Permitted()
        {
            var userId = _fixture.Create<int>();
            
            var requirement = _fixture.Create<DoorAuthorizationRequirement>();
            
            var principal = new ClaimsPrincipalBuilder()
                .WithUserId(userId)
                .WithClaim(ClaimTypes.Role, "employee")
                .Build();

            var ctx = new AuthorizationHandlerContext(new[] { requirement }, principal, null);

            _permissionsRepostoryMock.Setup(r => r.GetDoorPermissionsAsync(userId))
                .ReturnsAsync(new List<DoorPermission>
                {
                    new DoorPermission
                    {
                        DoorId = requirement.DoorId
                    }
                });

            await _handler.HandleAsync(ctx);
            
            Assert.True(ctx.HasSucceeded);
        }
        
        [Fact]
        public async Task Handle_DoesNot_Succeed_If_Door_IsNot_Permitted()
        {
            var userId = _fixture.Create<int>();
            
            var requirement = _fixture.Create<DoorAuthorizationRequirement>();
            
            var principal = new ClaimsPrincipalBuilder()
                .WithUserId(userId)
                .WithClaim(ClaimTypes.Role, "employee")
                .Build();

            var ctx = new AuthorizationHandlerContext(new[] { requirement }, principal, null);

            _permissionsRepostoryMock.Setup(r => r.GetDoorPermissionsAsync(userId))
                .ReturnsAsync(new List<DoorPermission>
                {
                    new DoorPermission
                    {
                        DoorId = requirement.DoorId + 1
                    }
                });

            await _handler.HandleAsync(ctx);
            
            Assert.False(ctx.HasSucceeded);
            Assert.False(ctx.HasFailed);
        }
        
        [Fact]
        public async Task Handle_DoesNot_Succeed_If_Not_Employee()
        {
            var userId = _fixture.Create<int>();
            
            var requirement = _fixture.Create<DoorAuthorizationRequirement>();
            
            var principal = new ClaimsPrincipalBuilder()
                .WithUserId(userId)
                .WithClaim(ClaimTypes.Role, "admin")
                .Build();

            var ctx = new AuthorizationHandlerContext(new[] { requirement }, principal, null);

            _permissionsRepostoryMock.Setup(r => r.GetDoorPermissionsAsync(userId))
                .ReturnsAsync(new List<DoorPermission>
                {
                    new DoorPermission
                    {
                        DoorId = requirement.DoorId
                    }
                });

            await _handler.HandleAsync(ctx);
            
            Assert.False(ctx.HasSucceeded);
            Assert.False(ctx.HasFailed);
        }
    }
}