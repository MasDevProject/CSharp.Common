using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasDev.Data
{
    public class NHibernateAsyncRepository<T> : NHibernateAsyncRepository, IAsyncRepository<T> where T : class, IModel, new()
    {
        public NHibernateAsyncRepository(IUnitOfWork uow) : base(uow)
        {
        }

        public virtual async Task ClearAsync()
        {
            await ClearAsync<T>();
        }

        public virtual async Task CreateAsync(IEnumerable<T> models)
        {
            await CreateAsync<T>(models);
        }

        public virtual async Task<int> CreateAsync(T model)
        {
            return await CreateAsync<T>(model);
        }

        public virtual async Task<IEnumerable<int>> CreateOrUpdateAsync(IEnumerable<T> models)
        {
            return await CreateOrUpdateAsync<T>(models);
        }

        public virtual async Task<int> CreateOrUpdateAsync(T model)
        {
            return await CreateOrUpdateAsync<T>(model);
        }

        public virtual async Task DeleteAsync(IEnumerable<T> models)
        {
            await DeleteAsync<T>(models);
        }

        public virtual async Task<int> DeleteAsync(T model)
        {
            return await DeleteAsync<T>(model);
        }

        public virtual async Task<T> ReadAsync(int id)
        {
            return await ReadAsync<T>(id);
        }

        public virtual async Task<IEnumerable<T>> ReadAsync(IEnumerable<int> ids)
        {
            return await ReadAsync<T>(ids);
        }

        public virtual async Task<T> ReadTransientAsync(int id)
        {
            return await ReadTransientAsync<T>(id);
        }

        public virtual async Task UpdateAsync(IEnumerable<T> models)
        {
            await UpdateAsync<T>(models);
        }

        public virtual async Task<int> UpdateAsync(T model)
        {
            return await UpdateAsync<T>(model);
        }

        public virtual async Task<T> UpdateAsync(int id, Action<T> updater)
        {
            return await UpdateAsync<T>(id, updater);
        }

        #region Sync

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

        #endregion
    }
}
