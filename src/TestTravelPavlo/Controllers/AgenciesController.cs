using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Services;
using Domain.Commands.AgencyCommands;
using Domain.Paging.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TestTravelPavlo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgenciesController : ControllerBase
    {

        private readonly ILogger<AgenciesController> _logger;
        private readonly IAgencyService _agencyservice;

        public AgenciesController(ILogger<AgenciesController> logger, IAgencyService agencyservice)
        {
            _logger = logger;
            _agencyservice = agencyservice;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var result = await _agencyservice.GetById(id);

                return Ok(result);
            }
            catch (NotFoundException<Guid>)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]PaginationFilter filter)
        {
            return Ok(await _agencyservice.FilterAsync(filter));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAgencyCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _agencyservice.CreateAsync(command);

            return Created($"/agencies/{result.Id}" ,result);
        }
    }
}