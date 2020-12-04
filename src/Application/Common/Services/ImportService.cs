using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Domain.Import;

namespace Application.Common.Services
{
    public interface IImportService<T> where T : class
    {
        IEnumerable<T> GetEntitiesFromFile(Stream fileStream);
        Task<ImportResult> Import(IEnumerable<T> entities);
    }
}