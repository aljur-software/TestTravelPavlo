using System.Collections.Generic;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Common.Services
{
    public interface IAgencyService
    {
        Task<IEnumerable<Agency>> GetAllAgencies();
    }
}
