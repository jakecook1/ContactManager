using System;
using ContactManagerWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerWeb.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IEfCoreRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;
        IEfCoreRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class, IEntity;

        int SaveChanges();
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        TContext Context { get; }
    }
}