using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MasDev.Data
{
    public class NHibernateAsyncRepository : NHibernateRepository, IAsyncRepository
    {
        public NHibernateAsyncRepository(IUnitOfWork uow) : base(uow)
        {
        }

        public virtual async Task<int> CreateAsync<T>(T model) where T : class, IModel, new()
        {
            return await Task.Factory.StartNew(() => Create(model));
        }

        public virtual async Task CreateAsync<T>(IEnumerable<T> models) where T : class, IModel, new()
        {
            await Task.Factory.StartNew(() => Create(models));
        }

        public virtual async Task<T> ReadAsync<T>(int id) where T : class, IModel, new()
        {
            return await Task.Factory.StartNew(() => Read<T>(id));
        }

        public virtual async Task<IEnumerable<T>> ReadAsync<T>(IEnumerable<int> ids) where T : class, IModel, new()
        {
            return await Task.Factory.StartNew(() => Read<T>(ids));
        }

        public async Task<T> ReadTransientAsync<T>(int id) where T : class, IModel, new()
        {
            return await Task.Factory.StartNew(() => ReadTransient<T>(id));
        }

        public virtual async Task<int> UpdateAsync<T>(T model) where T : class, IModel, new()
        {
            return await Task.Factory.StartNew(() => Update(model));
        }

        public virtual async Task UpdateAsync<T>(IEnumerable<T> models) where T : class, IModel, new()
        {
            await Task.Factory.StartNew(() => Update(models));
        }

        public async Task<T> UpdateAsync<T>(int id, Action<T> updater) where T : class, IModel, new()
        {
            var model = await ReadAsync<T>(id);
            if (model == null)
                return null;

            updater(model);
            await UpdateAsync(model);
            return model;
        }

        public virtual async Task<int> DeleteAsync<T>(T model) where T : class, IModel, new()
        {
            return await Task.Factory.StartNew(() => Delete(model));
        }

        public virtual async Task DeleteAsync<T>(IEnumerable<T> models) where T : class, IModel, new()
        {
            await Task.Factory.StartNew(() => Delete(models));
        }

        public async Task ClearAsync<T>() where T : class, IModel, new()
        {
            await Task.Factory.StartNew(() => Clear<T>());
        }

        public async Task<int> CreateOrUpdateAsync<T>(T model) where T : class, IModel, new()
        {
            return await Task.Factory.StartNew(() => CreateOrUpdate(model));
        }

        public async Task<IEnumerable<int>> CreateOrUpdateAsync<T>(IEnumerable<T> models) where T : class, IModel, new()
        {
            return await Task.Factory.StartNew(() => CreateOrUpdate(models));
        }
    }
}
