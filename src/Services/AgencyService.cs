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
        private readonly IRepository<Agency> _repository;

        public AgencyService(IRepository<Agency> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Agency>> GetAllAgencies()
        {
            return _repository.GetAll().Include(_ => _.Agents).ToList();
        }
    }
}
