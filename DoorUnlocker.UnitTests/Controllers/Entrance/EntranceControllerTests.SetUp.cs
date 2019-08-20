using AutoFixture;
using DoorUnlocker.API.Application.Domain.Repositories;
using DoorUnlocker.API.Controllers;
using DoorUnlocker.UnitTests.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Moq;

namespace DoorUnlocker.UnitTests.Controllers.Entrance
{
    public partial class EntranceControllerTests
    {
        private readonly Fixture _fixture = new Fixture();
        
        private EntranceController _controller;

        private Mock<IDoorsRepository> _doorsRepositoryMock;
        private Mock<IEntranceLogRepository> _entranceLogRepositoryMock;
        private Mock<IAuthorizationService> _authorizationServiceMock;
        
        public EntranceControllerTests()
        {
            _doorsRepositoryMock = new Mock<IDoorsRepository>();
            _entranceLogRepositoryMock = new Mock<IEntranceLogRepository>();
            _authorizationServiceMock = new Mock<IAuthorizationService>();
            
            _controller = new EntranceController(
                _doorsRepositoryMock.Object,
                _entranceLogRepositoryMock.Object,
                _authorizationServiceMock.Object,
                TestMapper.Instance.Value);
        }
    }
}