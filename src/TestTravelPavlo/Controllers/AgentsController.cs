using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Services;
using Domain.Commands.AgentCommands;
using Domain.Paging.Filters;
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
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                return Ok(await _agentService.GetById(id));
            }
            catch (NotFoundException<Guid>)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, Request.Path);
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilter filter)
        {
            try
            {
                return Ok(await _agentService.FilterAsync(filter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, Request.Path);
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAgentCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _agentService.CreateAsync(command);

                return Created($"/agent/{result.Id}", result);
            }
            catch(Exception e)
            {
                _logger.LogError(e, Request.Path);
                return new StatusCodeResult(500);
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
                return Ok(await _agentService.AddAgentToAgency(command));
            }
            catch (NotFoundException<Guid>)
            {
                return NotFound();
            }
            catch(Exception e)
            {
                _logger.LogError(e, Request.Path);
                return new StatusCodeResult(500);
            }
        }
    }
}