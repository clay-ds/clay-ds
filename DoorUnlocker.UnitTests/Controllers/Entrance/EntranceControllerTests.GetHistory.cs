using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using DoorUnlocker.API.Application.Domain.Models;
using DoorUnlocker.API.Infrastructure.Exceptions;
using DoorUnlocker.API.Models.Entrance;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DoorUnlocker.UnitTests.Controllers.Entrance
{
    public partial class EntranceControllerTests
    {
        [Fact]
        public async Task GetHistoryAsync_Throws_If_Door_Not_Found()
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
        public async Task GetHistoryAsync_Throws_If_OfficeId_DoesNot_Match()
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
        public async Task GetHistoryAsync_Returns_All_History_Logs()
        {
            var door = new Door
            {
                Id = _fixture.Create<int>(),
                OfficeId = _fixture.Create<int>()
            };

            var logs = _fixture.CreateMany<EntranceLogEntry>().ToList();
            var request = _fixture.Create<GetHistoryRequest>();
            
            _doorsRepositoryMock.Setup(r => r.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(door);

            _entranceLogRepositoryMock.Setup(r => r.GetLastForDoorAsync(door.Id, request.Count))
                .ReturnsAsync(logs);

            var result = await _controller.GetHistoryAsync(door.OfficeId, door.Id, request);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var history = Assert.IsType<List<EntranceHistoryModel>>(okResult.Value);
            Assert.Equal(logs.Count, history.Count);
        }
    }
}