using System.Linq;

namespace Application.Common.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> GetPaged<T>(this IQueryable<T> query,int page, int pageSize) 
            where T : class
        {
            var skip = (page - 1) * pageSize;

            return query.Skip(skip).Take(pageSize);
        }
    }
}
