using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ContactManagerWeb.Data.Paging;
using ContactManagerWeb.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace ContactManagerWeb.Data
{
    public interface IEfCoreRepository<T> : IDisposable where T : class, IEntity
    {
        IQueryable<T> Queryable(string sql, params object[] parameters);

        T Search(params object[] keyValues);

        T Single(Expression<Func<T, bool>> predicate = null,
                            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                            bool disableTracking = true);

        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null,
                              Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                              Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                              bool disableTracking = true);

        IPaginate<T> GetList(Expression<Func<T, bool>> predicate = null,
                             Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                             Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                             int index = 0,
                             int size = 10,
                             bool disableTracking = true);
        
        void Add(T entity);
        void Add(params T[] entities);
        void Add(IEnumerable<T> entities);

        void Update(T entity);
        void Update(params T[] entities);
        void Update(IEnumerable<T> entities);

        void Delete(T entity);
        void Delete(object id);
        void Delete(params T[] entities);
        void Delete(IEnumerable<T> entities);
    }
}