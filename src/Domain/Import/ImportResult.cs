using System.Collections.Generic;

namespace Domain.Import
{
    public class ImportResult
    {
        public IList<object> SuccessfullyImported { get; } = new List<object>();
        public IList<object> NotImported { get; } = new List<object>();
    }
}
