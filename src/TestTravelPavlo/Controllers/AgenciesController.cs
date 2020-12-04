using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Services;
using Domain.Commands.AgencyCommands;
using Domain.Entities;
using Domain.Paging.Filters;
using Microsoft.AspNetCore.Http;
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
        private readonly IImportService<Agency> _importService;


        public AgenciesController(ILogger<AgenciesController> logger, IAgencyService agencyservice, IImportService<Agency> importService)
        {
            _logger = logger;
            _agencyservice = agencyservice;
            _importService = importService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                return Ok(await _agencyservice.GetById(id));
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

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var importResult = await _importService.Import(_importService.GetEntitiesFromFile(stream));
                return Ok(importResult);
            }
        }
    }
}