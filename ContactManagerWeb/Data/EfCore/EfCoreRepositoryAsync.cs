using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ContactManagerWeb.Data.Paging;
using ContactManagerWeb.Helpers;
using ContactManagerWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ContactManagerWeb.Data
{
    public class EfCoreRepositoryAsync<T> : IEfCoreRepositoryAsync<T> where T : class, IEntity
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public EfCoreRepositoryAsync(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public Task<T> SingleAsync(Expression<Func<T, bool>> predicate = null,
                                   Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                   bool disableTracking = true)
        {
            IQueryable<T> query = _dbSet;

            if (disableTracking) query = query.AsNoTracking();
            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);

            return query.FirstOrDefaultAsync();
        }

        public Task<IPaginate<T>> GetListAsync(Expression<Func<T, bool>> predicate = null,
                                      Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                      Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                      int index = 0,
                                      int size = 10,
                                      bool disableTracking = true,
                                      CancellationToken cancellationToken = default(CancellationToken))
        {
            IQueryable<T> query = _dbSet;

            if (disableTracking) query = query.AsNoTracking();
            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);
            if (orderBy != null) query = orderBy(query);

            return query.ToPaginateAsync(index, size);
        }
        
        public Task AddAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Add date stamps
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            return Task.FromResult(_dbSet.AddAsync(entity, cancellationToken));
        }

        public Task AddAsync(T entity)
        {
            return AddAsync(entity, new CancellationToken());
        }

        public Task AddAsync(params T[] entities)
        {
            foreach (var entity in entities)
            {
                // Add date stamps
                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;
            }

            return _dbSet.AddRangeAsync(entities);
        }

        public Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var entity in entities)
            {
                // Add date stamps
                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;
            }

            return _dbSet.AddRangeAsync(entities, cancellationToken);
        }
    }
}