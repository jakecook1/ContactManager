using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ContactManagerWeb.Data.Paging;
using ContactManagerWeb.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace ContactManagerWeb.Data
{
    public interface IEfCoreRepositoryAsync<T> where T : class, IEntity
    {
        Task<T> SingleAsync(Expression<Func<T, bool>> predicate = null,
                            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                            bool disableTracking = true);

        Task<IPaginate<T>> GetListAsync(Expression<Func<T, bool>> predicate = null,
                               Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                               Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                               int index = 0,
                               int size = 10,
                               bool disableTracking = true,
                               CancellationToken cancellationToken = default(CancellationToken));
        
        Task AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));
        Task AddAsync(params T[] entities);
        Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken));
    }
}