using System;
using System.Collections.Generic;
using System.Linq;

namespace MasDev.Data
{
    public class NHibernateRepository<T> : NHibernateRepository, IRepository<T> where T : class, IModel, new()
    {
        public NHibernateRepository(IUnitOfWork uow) : base(uow)
        {
        }

        public virtual IQueryable<T> Query { get { return QueryFor<T>(); } }

        public virtual void Clear()
        {
            Clear<T>();
        }

        public virtual void Create(IEnumerable<T> models)
        {
            Create<T>(models);
        }

        public virtual int Create(T model)
        {
            return Create<T>(model);
        }

        public virtual IEnumerable<int> CreateOrUpdate(IEnumerable<T> models)
        {
            return CreateOrUpdate<T>(models);
        }

        public virtual int CreateOrUpdate(T model)
        {
            return CreateOrUpdate<T>(model);
        }

        public virtual void Delete(IEnumerable<T> models)
        {
            Delete<T>(models);
        }

        public virtual int Delete(T model)
        {
            return Delete<T>(model);
        }

        public virtual void Lock(T model, LockMode lockMode)
        {
            Lock<T>(model, lockMode);
        }

        public virtual IEnumerable<T> Read(IEnumerable<int> ids)
        {
            return Read<T>(ids);
        }

        public virtual T Read(int id)
        {
            return Read<T>(id);
        }

        public virtual T ReadTransient(int id)
        {
            return ReadTransient<T>(id);
        }

        public virtual void Update(IEnumerable<T> models)
        {
            Update<T>(models);
        }

        public virtual int Update(T model)
        {
            return Update<T>(model);
        }

        public virtual T Update(int id, Action<T> updater)
        {
            return Update<T>(id, updater);
        }
    }
}
