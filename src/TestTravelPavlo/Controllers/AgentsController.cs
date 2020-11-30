using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Services;
using Domain.Commands.AgentCommands;
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

        public AgentsController(ILogger<AgentsController> logger, IAgentService agentService)
        {
            _logger = logger;
            _agentService = agentService;
        }

        [HttpGet]
        [Route("{Id?}")]
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
        public IActionResult Get()
        {
            return Ok(_agentService.GetAll());
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