using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Services;
using AutoMapper;
using Domain.Commands.AgentCommands;
using Domain.Entities;
using Domain.Paging.Filters;
using Domain.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class AgentService : IAgentService
    {
        private readonly IRepository<Agent> _agentRepository;
        private readonly IAgencyService _agencyService;
        private readonly IMapper _mapper;

        public AgentService(IRepository<Agent> agentRepository, IAgencyService agencyService, IMapper mapper)
        {
            _agentRepository = agentRepository;
            _agencyService = agencyService;
            _mapper = mapper;
        }

        public async Task<bool> AddAgentToAgency(AddAgentToAgencyCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            var agency = await _agencyService.GetById(command.AgencyId);
            var agent = await GetById(command.AgentId);
            agent.Agencies.Add(agency);
            var affectedRecordsCount = await _agentRepository.UpdateRecordAsync(agent);

            return affectedRecordsCount >= 1;
        }

        public async Task<Agent> CreateAsync(CreateAgentCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            var agency = _mapper.Map<Agent>(command);
            var result = await _agentRepository.CreateRecordAsync(agency);

            return result;
        }

        public async Task<IEnumerable<Agent>> GetAll()
        {
            return _agentRepository.GetAll()
                .AsQueryable()
                .ToList();
        }

        public async Task<Agent> GetById(Guid id)
        {
            if(id == Guid.Empty)
                throw new ArgumentException(nameof(id));
            var agent = _agentRepository.FindBy(_ => _.Id == id)
                .AsQueryable()
                .Include(_ => _.Agencies)
                .FirstOrDefault();
            if (agent == null)
                throw new NotFoundException<Guid>(nameof(Agent), id);

            return agent;
        }

        public async Task<PagedResponse<IEnumerable<Agent>>> FilterAsync(PaginationFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            return _agentRepository.PaginationAsync(filter);
        }
    }
}