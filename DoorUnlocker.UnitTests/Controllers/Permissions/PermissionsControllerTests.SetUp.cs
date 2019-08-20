using AutoFixture;
using DoorUnlocker.API.Application.Domain.Repositories;
using DoorUnlocker.API.Controllers;
using DoorUnlocker.UnitTests.Infrastructure;
using Moq;

namespace DoorUnlocker.UnitTests.Controllers.Permissions
{
    public partial class PermissionsControllerTests
    {
        private readonly Fixture _fixture = new Fixture();

        private PermissionsController _controller;

        private Mock<IPermissionsRepository> _permissionsRepositoryMock;
        private Mock<IDoorsRepository> _doorsRepositoryMock;
        
        public PermissionsControllerTests()
        {
            _permissionsRepositoryMock = new Mock<IPermissionsRepository>();
            _doorsRepositoryMock = new Mock<IDoorsRepository>();
            
            _controller = new PermissionsController(
                _permissionsRepositoryMock.Object,
                _doorsRepositoryMock.Object,
                TestMapper.Instance.Value);
        }
    }
}