using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void SaveChanges();

        IEnumerable<T> GetAll();

        void Add(T entity);

        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
    }
}
