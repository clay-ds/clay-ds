using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DoorUnlocker.API.Application.Domain.Models;
using DoorUnlocker.API.Application.Domain.Repositories;
using DoorUnlocker.API.Infrastructure.Configuration;
using DoorUnlocker.API.Infrastructure.Exceptions;
using DoorUnlocker.API.Models.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoorUnlocker.API.Controllers
{
    [Route("api/users")]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionsRepository _permissionsRepository;
        private readonly IDoorsRepository _doorsRepository;
        private readonly IMapper _mapper;

        public PermissionsController(
            IPermissionsRepository permissionsRepository,
            IDoorsRepository doorsRepository,
            IMapper mapper)
        {
            _permissionsRepository = permissionsRepository;
            _doorsRepository = doorsRepository;
            _mapper = mapper;
        }

        [HttpGet("{userId:int}/door-permissions")]
        [Authorize(nameof(AuthorizationPolicies.AdminOnly))]
        public async Task<ActionResult<UserPermittedDoorsModel>> GetForUserAsync(int userId)
        {
            var permissions = await _permissionsRepository.GetDoorPermissionsAsync(userId);

            var result = new UserPermittedDoorsModel
            {
                UserId = userId,
                PermittedDoors = _mapper.Map<List<PermittedDoorModel>>(permissions)
            };

            return Ok(result);
        }

        [HttpPut("{userId:int}/door-permissions")]
        [Authorize(nameof(AuthorizationPolicies.AdminOnly))]
        public async Task<ActionResult> UpdateAsync(int userId, [FromBody] UpdateDoorPermissionsRequest request)
        {
            var existingPermissions = await _permissionsRepository.GetDoorPermissionsAsync(userId);

            var toDelete = existingPermissions
                .Where(p => !request.PermittedDoors.Contains(p.DoorId));

            var toAddIds = request.PermittedDoors
                .Where(d => existingPermissions.All(p => p.DoorId != d))
                .ToList();

            var allExist = await _doorsRepository.AllExistAsync(toAddIds);
            if (!allExist)
                throw ApiException.BadRequest("Some doors don't exist");

            var toAdd = toAddIds
                .Select(d => new DoorPermission
                {
                    DoorId = d,
                    UserId = userId
                });

            await _permissionsRepository.UpdateDoorPermissionsAsync(new UpdateDoorPermissionsModel
            {
                ToAdd = toAdd,
                ToDelete = toDelete
            });

            return Ok();
        }
    }
}