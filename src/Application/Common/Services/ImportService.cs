using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Domain.Import;

namespace Application.Common.Services
{
    public interface IImportService<T> where T : class
    {
        ICollection<T> GetEntitiesFromFile(Stream fileStream);
        Task<bool> Import(ICollection<T> entities);
    }
}