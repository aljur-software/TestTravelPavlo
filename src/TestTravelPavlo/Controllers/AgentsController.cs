using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Services;
using Domain.Commands.AgentCommands;
using Domain.Entities;
using Domain.Paging.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TestTravelPavlo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgentsController : ControllerBase
    {
        private readonly ILogger<AgentsController> _logger;
        private readonly IAgentService _agentService;
        private readonly IImportService<Agent> _importService;

        public AgentsController(ILogger<AgentsController> logger, IAgentService agentService, IImportService<Agent> importService)
        {
            _logger = logger;
            _agentService = agentService;
            _importService = importService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var result = await _agentService.GetById(id);

                return Ok(result);
            }
            catch (NotFoundException<Guid>)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilter filter)
        {
            return Ok(await _agentService.FilterAsync(filter));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAgentCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _agentService.CreateAsync(command);

            return Created($"/agent/{result.Id}", result);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var entitiesFromFile = _importService.GetEntitiesFromFile(stream);
                var importResult = await _importService.Import(entitiesFromFile);

                return Ok(importResult);
            }
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> AddToAgency(AddAgentToAgencyCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _agentService.AddAgentToAgency(command);

                return Ok(result);
            }
            catch (NotFoundException<Guid>)
            {
                return NotFound();
            }
        }
    }
}