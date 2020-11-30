using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Commands.AgentCommands;
using Domain.Entities;

namespace Application.Common.Services
{
    public interface IAgentService
    {
        Task<Agent> CreateAsync(CreateAgentCommand command);
        Task<bool> AddAgentToAgency(AddAgentToAgencyCommand command);
        Task<IEnumerable<Agent>> GetAll();
        Task<Agent> GetById(Guid Id);
    }
}