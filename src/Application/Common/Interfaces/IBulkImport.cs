using System.Collections.Generic;

namespace Application.Common.Interfaces
{
    public interface IBulkImport<T>
    {
        bool Import(IEnumerable<T> records);
    }
}
