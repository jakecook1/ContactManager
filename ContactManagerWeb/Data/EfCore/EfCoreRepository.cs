using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ContactManagerWeb.Data.Paging;
using ContactManagerWeb.Helpers;
using ContactManagerWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ContactManagerWeb.Data
{
    public class EfCoreRepository<T> : IEfCoreRepository<T> where T : class, IEntity
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public EfCoreRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> Queryable(string sql, params object[] parameters) => _dbSet.FromSqlRaw(sql, parameters);

        public T Search(params object[] keyValues) => _dbSet.Find(keyValues);

        public T Single(Expression<Func<T, bool>> predicate = null, 
                        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                        bool disableTracking = true)
        {
            IQueryable<T> query = _dbSet;

            if (disableTracking) query = query.AsNoTracking();
            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null,
                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                     Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                     bool disableTracking = true)
        {
            IQueryable<T> query = _dbSet;

            if (disableTracking) query = query.AsNoTracking();
            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);
            if (orderBy != null) query = orderBy(query);

            return query.AsEnumerable();
        }

        public IPaginate<T> GetList(Expression<Func<T, bool>> predicate = null,
                                    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                    Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                    int index = 0,
                                    int size = 10,
                                    bool disableTracking = true)
        {
            IQueryable<T> query = _dbSet;

            if (disableTracking) query = query.AsNoTracking();
            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);
            if (orderBy != null) query = orderBy(query);

            return query.ToPaginate(index, size);
        }
        
        public void Add(T entity)
        {
            // Add date stamps
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            _dbSet.Add(entity);
        }

        public void Add(params T[] entities)
        {
            foreach (var entity in entities)
            {
                // Add date stamps
                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;
            }

            _dbSet.AddRange(entities);
        }

        public void Add(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                // Add date stamps
                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;
            }

            _dbSet.AddRange(entities);
        }

        public void Update(T entity)
        {
            // Add date stamps
            entity.UpdatedAt = DateTime.UtcNow;

            _dbSet.Update(entity);
        }

        public void Update(params T[] entities)
        {
            foreach (var entity in entities)
            {
                // Add date stamps
                entity.UpdatedAt = DateTime.UtcNow;
            }

            _dbSet.UpdateRange(entities);
        }

        public void Update(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                // Add date stamps
                entity.UpdatedAt = DateTime.UtcNow;
            }
            
            _dbSet.UpdateRange(entities);
        }

        public void Delete(T entity)
        {
            T existing = _dbSet.Find(entity);
            
            if (existing != null)
                _dbSet.Remove(existing);
        }

        public void Delete(object id)
        {
            var typeInfo = typeof(T).GetTypeInfo();
            var key = _context.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
            var property = typeInfo.GetProperty(key?.Name);

            if (property != null)
            {
                var entity = Activator.CreateInstance<T>();
                property.SetValue(entity, id);
                _context.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                var entity = _dbSet.Find(id);

                if (entity != null)
                    Delete(entity);
            }
        }

        public void Delete(params T[] entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Delete(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}