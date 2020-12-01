using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void SaveChanges();

        Task SaveChangesAsync();

        IQueryable<T> GetAll();

        void Add(T entity);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
    }
}
