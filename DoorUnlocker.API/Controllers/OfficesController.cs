using System.Threading.Tasks;
using AutoMapper;
using DoorUnlocker.API.Application.Domain.Models;
using DoorUnlocker.API.Application.Domain.Repositories;
using DoorUnlocker.API.Infrastructure.Configuration;
using DoorUnlocker.API.Infrastructure.Exceptions;
using DoorUnlocker.API.Models.Offices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoorUnlocker.API.Controllers
{
    [Route("api/offices")]
    public class OfficesController : ControllerBase
    {
        private readonly IOfficesRepository _officesRepository;
        private readonly IMapper _mapper;

        public OfficesController(
            IOfficesRepository officesRepository,
            IMapper mapper)
        {
            _officesRepository = officesRepository;
            _mapper = mapper;
        }

        [HttpGet("{officeId:int}")]
        public async Task<ActionResult> GetOfficeAsync(int officeId)
        {
            var office = await _officesRepository.GetAsync(officeId);

            if (office == null)
            {
                throw ApiException.NotFound("Office not found");
            }

            return Ok(_mapper.Map<OfficeModel>(office));
        }

        [HttpPost]
        [Authorize(nameof(AuthorizationPolicies.AdminOnly))]
        public async Task<ActionResult> CreateOfficeAsync([FromBody] CreateOfficeRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var office = _mapper.Map<Office>(request);

            var result = await _officesRepository.AddAsync(office);

            return Ok(new CreateOfficeResponse
            {
                OfficeId = result
            });
        }

        [HttpPost("{officeId:int}/doors")]
        [Authorize(nameof(AuthorizationPolicies.AdminOnly))]
        public async Task<ActionResult> CreateDoorAsync(int officeId, [FromBody] CreateDoorRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var office = await _officesRepository.GetAsync(officeId);

            if (office == null)
            {
                throw ApiException.NotFound("Office not found");
            }

            var door = _mapper.Map<Door>(request);

            var doorId = await _officesRepository.AddDoorAsync(office, door);

            return Ok(new CreateDoorResponse
            {
                DoorId = doorId
            });
        }

        [HttpDelete("{officeId:int}")]
        [Authorize(nameof(AuthorizationPolicies.AdminOnly))]
        public async Task<ActionResult> DeleteOfficeAsync(int officeId)
        {
            var office = await _officesRepository.GetAsync(officeId);

            if (office == null)
            {
                throw ApiException.NotFound("Office not found");
            }

            await _officesRepository.DeleteAsync(office);

            return Ok();
        }
    }
}