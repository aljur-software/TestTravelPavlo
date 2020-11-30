using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class AgencyService : IAgencyService
    {
        private readonly IRepository<Agency> _agencyRepository;

        public AgencyService(IRepository<Agency> agencyRepository)
        {
            _agencyRepository = agencyRepository;
        }

        public async Task<IEnumerable<Agency>> GetAllAgencies()
        {
            return  _agencyRepository.GetAll()
                .AsQueryable()
                .Include(_ => _.Agents)
                .ToList();
        }
    }
}
