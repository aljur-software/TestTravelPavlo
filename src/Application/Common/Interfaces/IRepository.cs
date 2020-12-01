using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<T> CreateRecordAsync(T record, CancellationToken cancellationToken = default);
        Task<int> UpdateRecordAsync(T record, CancellationToken cancellationToken = default);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
    }
}