using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Services;
using AutoMapper;
using Domain.Commands.AgencyCommands;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class AgencyService : IAgencyService
    {
        private readonly IRepository<Agency> _agencyRepository;
        private readonly IMapper _mapper;

        public AgencyService(IRepository<Agency> agencyRepository, IMapper mapper)
        {
            _agencyRepository = agencyRepository;
            _mapper = mapper;
        }

        public async Task<Agency> CreateAsync(CreateAgencyCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            var agency = _mapper.Map<Agency>(command);
            var result = await _agencyRepository.CreateRecordAsync(agency);
            return result;
        }

        public async Task<IEnumerable<Agency>> GetAll()
        {
            return  _agencyRepository.GetAll()
                .AsQueryable()
                .Include(_ => _.Agents)
                .ToList();
        }

        public async Task<Agency> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException(nameof(id));

            var agency = _agencyRepository.FindBy(_ => _.Id == id)
                .AsQueryable()
                .Include(_ => _.Agents)
                .FirstOrDefault();

            if (agency == null)
                throw new NotFoundException<Guid>(nameof(Agency), id);

            return agency;
        }
    }
}