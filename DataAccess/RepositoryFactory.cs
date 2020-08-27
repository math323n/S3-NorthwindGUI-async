using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class RepositoryFactory<TRepository, TEntity>
        where TEntity : class
        where TRepository : IRepository<TEntity>, new()
    {
        private static readonly RepositoryFactory<TRepository, TEntity> instance;
        protected NorthwindContext northwindContext;

        private RepositoryFactory() { }

        public static RepositoryFactory<TRepository, TEntity> GetInstance()
        {
            return instance is null ? new RepositoryFactory<TRepository, TEntity>() : instance;
        }

        public virtual TRepository Create()
        {
            if(northwindContext is null)
                northwindContext = new NorthwindContext();
            TRepository repo = new TRepository();
            repo.Context = northwindContext;
            return repo;
        }

        public virtual void KillContext()
        {
            northwindContext.Dispose();
        }
    }
}
