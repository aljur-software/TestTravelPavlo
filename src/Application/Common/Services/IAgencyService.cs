using System;
using System.Collections.Generic;
using Domain.Entities;
using System.Threading.Tasks;
using Domain.Commands.AgencyCommands;

namespace Application.Common.Services
{
    public interface IAgencyService
    {
        Task<IEnumerable<Agency>> GetAll();
        Task<Agency> CreateAsync(CreateAgencyCommand command);
        Task<Agency> GetById(Guid Id);
    }
}