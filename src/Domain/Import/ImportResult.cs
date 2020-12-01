using System.Collections.Generic;

namespace Domain.Import
{
    public class ImportResult<T>
    {
        public IList<T> SuccessfullyImported { get; set; } = new List<T>();
        public IList<T> NotImported { get; set; } = new List<T>();
    }
}
