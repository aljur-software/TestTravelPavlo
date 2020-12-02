using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Paging.Filters;
using Domain.Wrappers;
using Infractructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Common.Extensions;
using Domain.Paging;

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

        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.AsNoTracking();
        }

        public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public async Task<T> CreateRecordAsync(T record, CancellationToken cancellationToken = default)
        {
            var result = await _context.AddAsync(record, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return result.Entity;
        }

        public async Task<int> UpdateRecordAsync(T record, CancellationToken cancellationToken = default)
        {
            var result = _context.Attach(record);
            result.State = EntityState.Modified;

            return await _context.SaveChangesAsync(cancellationToken);
        }

        public PagedResponse<IEnumerable<T>> PaginationAsync(PaginationFilter filter)
        {
            var entries = _dbSet.GetPaged(filter.PageNumber, filter.PageSize);
            var totalEntries = _dbSet.Count();
            var pagingModel = new PagingModel()
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalEntries / filter.PageSize),
                TotalEntries = totalEntries
            };
            var result = new PagedResponse<IEnumerable<T>>(entries, pagingModel);

            return result;
        }
    }
}