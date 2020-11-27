using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Infractructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infractructure.Repositories
{
    public class TravelRepository<T> : IRepository<T> where T: class
    {
        private readonly DbSet<T> _dbSet;
        private readonly ApplicationDbContext _context;
      
        public TravelRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
       
        public virtual IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking();
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }

        public virtual Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
