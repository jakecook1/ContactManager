using System;
using ContactManagerWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerWeb.Data
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IUnitOfWork
        where TContext : DbContext, IDisposable
    {
        public TContext Context { get; }

        public UnitOfWork(TContext context) => Context = context;

        public int SaveChanges() => Context.SaveChanges();

        public void Dispose() => Context.Dispose();

        public IEfCoreRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
        {
            return (IEfCoreRepository<TEntity>) new EfCoreRepository<TEntity>(Context);
        }

        public IEfCoreRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class, IEntity
        {
            return (IEfCoreRepositoryAsync<TEntity>) new EfCoreRepositoryAsync<TEntity>(Context);
        }
    }
}