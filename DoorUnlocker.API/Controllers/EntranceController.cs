using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DoorUnlocker.API.Application.Domain.Models;
using DoorUnlocker.API.Application.Domain.Repositories;
using DoorUnlocker.API.Application.Services.Authorization;
using DoorUnlocker.API.Infrastructure.Configuration;
using DoorUnlocker.API.Infrastructure.Exceptions;
using DoorUnlocker.API.Infrastructure.Helpers;
using DoorUnlocker.API.Models.Entrance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoorUnlocker.API.Controllers
{
    [Route("api/offices/{officeId}/doors/{doorId}")]
    public class EntranceController : ControllerBase
    {
        private readonly IDoorsRepository _doorsRepository;
        private readonly IEntranceLogRepository _entranceLogRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        public EntranceController(
            IDoorsRepository doorsRepository,
            IEntranceLogRepository entranceLogRepository,
            IAuthorizationService authorizationService,
            IMapper mapper)
        {
            _doorsRepository = doorsRepository;
            _entranceLogRepository = entranceLogRepository;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }

        [HttpPost("entrance")]
        public async Task<ActionResult> EnterDoorAsync(int officeId, int doorId)
        {
            var door = await GetAndEnsureDoorAsync(officeId, doorId);

            var accessResult = await _authorizationService.AuthorizeAsync(
                User,
                null,
                new DoorAuthorizationRequirement
                {
                    DoorId = doorId
                }
            );

            if (!accessResult.Succeeded)
            {
                throw ApiException.Forbidden("Access denied");
            }

            var entranceLog = _mapper.MultipleMap<EntranceLogEntry>(door, User);
            await _entranceLogRepository.AddLogAsync(entranceLog);

            return Ok();
        }

        [HttpGet("history")]
        [Authorize(nameof(AuthorizationPolicies.AdminOnly))]
        public async Task<ActionResult<IList<EntranceHistoryModel>>> GetHistoryAsync(
            int officeId,
            int doorId,
            [FromQuery] GetHistoryRequest request)
        {
            var door = await GetAndEnsureDoorAsync(officeId, doorId);

            var logs = await _entranceLogRepository.GetLastForDoorAsync(door.Id, request.Count);
            var history = _mapper.Map<List<EntranceHistoryModel>>(logs);

            return Ok(history);
        }

        private async Task<Door> GetAndEnsureDoorAsync(int officeId, int doorId)
        {
            var door = await _doorsRepository.GetAsync(doorId);

            if (door == null)
            {
                throw ApiException.NotFound("Door not found");
            }

            if (door.OfficeId != officeId)
            {
                throw ApiException.NotFound("Door not found");
            }

            return door;
        }
    }
}