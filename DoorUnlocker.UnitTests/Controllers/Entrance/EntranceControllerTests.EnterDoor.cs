using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture;
using DoorUnlocker.API.Application.Domain.Models;
using DoorUnlocker.API.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DoorUnlocker.UnitTests.Controllers.Entrance
{
    public partial class EntranceControllerTests
    {
        [Fact]
        public async Task EnterDoorAsync_Throws_If_Door_Not_Found()
        {
            var doorId = _fixture.Create<int>();
            var officeId = _fixture.Create<int>();
            
            _doorsRepositoryMock.Setup(r => r.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(default(Door));

            var exception = await Assert.ThrowsAsync<ApiException>(
                () => _controller.EnterDoorAsync(officeId, doorId));
            
            Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        }

        [Fact]
        public async Task EnterDoorAsync_Throws_If_OfficeId_DoesNot_Match()
        {
            var officeId = _fixture.Create<int>();
            
            var door = new Door
            {
                Id = _fixture.Create<int>(),
                OfficeId = officeId + 1
            };
            
            _doorsRepositoryMock.Setup(r => r.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(door);

            var exception = await Assert.ThrowsAsync<ApiException>(
                () => _controller.EnterDoorAsync(officeId, door.Id));
            
            Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        }

        [Fact]
        public async Task EnterDoorAsync_Throws_If_No_Access()
        {
            var door = new Door
            {
                Id = _fixture.Create<int>(),
                OfficeId = _fixture.Create<int>()
            };
            
            _doorsRepositoryMock.Setup(r => r.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(door);

            _authorizationServiceMock
                .Setup(s => s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Failed());

            var exception = await Assert.ThrowsAsync<ApiException>(
                () => _controller.EnterDoorAsync(door.OfficeId, door.Id));
            
            Assert.Equal(HttpStatusCode.Forbidden, exception.StatusCode);
        }
        
        [Fact]
        public async Task EnterDoorAsync_Returns_Ok_If_Has_Access()
        {
            var door = new Door
            {
                Id = _fixture.Create<int>(),
                OfficeId = _fixture.Create<int>()
            };
            
            _doorsRepositoryMock.Setup(r => r.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(door);

            _authorizationServiceMock
                .Setup(s => s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Success);

            var result = await _controller.EnterDoorAsync(door.OfficeId, door.Id);
            
            Assert.IsType<OkResult>(result);
        }
        
        [Fact]
        public async Task EnterDoorAsync_DoesNot_Save_Log_If_No_Access()
        {
            var door = new Door
            {
                Id = _fixture.Create<int>(),
                OfficeId = _fixture.Create<int>()
            };
            
            _doorsRepositoryMock.Setup(r => r.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(door);

            _authorizationServiceMock
                .Setup(s => s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Failed());

            await Assert.ThrowsAsync<ApiException>(() => _controller.EnterDoorAsync(door.OfficeId, door.Id));
            
            _entranceLogRepositoryMock.Verify(r => r.AddLogAsync(It.IsAny<EntranceLogEntry>()), Times.Never);
        }
        
        [Fact]
        public async Task EnterDoorAsync_Saves_Log_If_Has_Access()
        {
            var door = new Door
            {
                Id = _fixture.Create<int>(),
                OfficeId = _fixture.Create<int>()
            };
            
            _doorsRepositoryMock.Setup(r => r.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(door);

            _authorizationServiceMock
                .Setup(s => s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .ReturnsAsync(AuthorizationResult.Success);

            await _controller.EnterDoorAsync(door.OfficeId, door.Id);
            
            _entranceLogRepositoryMock.Verify(r => r.AddLogAsync(It.IsAny<EntranceLogEntry>()), Times.Once);
        }
    }
}