using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using DoorUnlocker.API.Application.Domain.Models;
using DoorUnlocker.API.Infrastructure.Exceptions;
using DoorUnlocker.API.Models.Permissions;
using Moq;
using Xunit;

namespace DoorUnlocker.UnitTests.Controllers.Permissions
{
    public partial class PermissionsControllerTests
    {
        [Fact]
        public async Task UpdateAsync_Throws_If_Door_DoesNot_Exist()
        {
            var userId = _fixture.Create<int>();

            var permissionIds = _fixture.CreateMany<int>().ToList();

            var permissions = BuildPermissions(userId, permissionIds);

            _permissionsRepositoryMock.Setup(r => r.GetDoorPermissionsAsync(userId))
                .ReturnsAsync(permissions);

            _doorsRepositoryMock.Setup(r => r.AllExistAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(false);

            var request = new UpdateDoorPermissionsRequest
            {
                PermittedDoors = permissionIds
            };

            var exception = await Assert.ThrowsAsync<ApiException>(() => _controller.UpdateAsync(userId, request));
            Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
        }

        [Fact]
        public async Task UpdateAsync_Deletes_Missing_Permissions()
        {
            var userId = _fixture.Create<int>();

            var toDelete = _fixture.CreateMany<int>().ToList();
            var toLeave = _fixture.CreateMany<int>().ToList();

            var permissions = BuildPermissions(userId, toDelete.Concat(toLeave));

            _permissionsRepositoryMock.Setup(r => r.GetDoorPermissionsAsync(userId))
                .ReturnsAsync(permissions);

            _doorsRepositoryMock.Setup(r => r.AllExistAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(true);

            var request = new UpdateDoorPermissionsRequest
            {
                PermittedDoors = toLeave
            };

            await _controller.UpdateAsync(userId, request);

            var verification = Expr(m =>
                !m.ToAdd.Any() &&
                m.ToDelete.Count() == toDelete.Count &&
                m.ToDelete.All(p => toDelete.Contains(p.DoorId)));
            
            _permissionsRepositoryMock.Verify(
                r => r.UpdateDoorPermissionsAsync(It.Is(verification)),
                Times.Once);
        }
        
        [Fact]
        public async Task UpdateAsync_Adds_New_Permissions()
        {
            var userId = _fixture.Create<int>();

            var toAdd = _fixture.CreateMany<int>().ToList();
            var toLeave = _fixture.CreateMany<int>().ToList();

            var permissions = BuildPermissions(userId, toLeave);

            _permissionsRepositoryMock.Setup(r => r.GetDoorPermissionsAsync(userId))
                .ReturnsAsync(permissions);

            _doorsRepositoryMock.Setup(r => r.AllExistAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(true);

            var request = new UpdateDoorPermissionsRequest
            {
                PermittedDoors = toLeave.Concat(toAdd).ToList()
            };

            await _controller.UpdateAsync(userId, request);

            var verification = Expr(m =>
                !m.ToDelete.Any() &&
                m.ToAdd.Count() == toAdd.Count &&
                m.ToAdd.All(p => toAdd.Contains(p.DoorId)));
            
            _permissionsRepositoryMock.Verify(
                r => r.UpdateDoorPermissionsAsync(It.Is(verification)),
                Times.Once);
        }

        private List<DoorPermission> BuildPermissions(int userId, IEnumerable<int> perms)
        {
            return perms.Select(perm => new DoorPermission
            {
                DoorId = perm,
                UserId = userId
            }).ToList();
        }

        private Expression<Func<UpdateDoorPermissionsModel, bool>> Expr(Expression<Func<UpdateDoorPermissionsModel, bool>> expr)
            => expr;
    }
}