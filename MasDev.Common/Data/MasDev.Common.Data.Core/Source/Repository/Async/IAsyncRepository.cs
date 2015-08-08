using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MasDev.Data
{
    public interface IAsyncRepository : IRepository
    {
        Task<int> CreateAsync<TModel>(TModel model) where TModel : class, IModel, new();

        Task CreateAsync<TModel>(IEnumerable<TModel> models) where TModel : class, IModel, new();

        Task<int> CreateOrUpdateAsync<TModel>(TModel model) where TModel : class, IModel, new();

        Task<IEnumerable<int>> CreateOrUpdateAsync<TModel>(IEnumerable<TModel> models) where TModel : class, IModel, new();

        Task<TModel> ReadTransientAsync<TModel>(int id) where TModel : class, IModel, new();

        Task<TModel> ReadAsync<TModel>(int id) where TModel : class, IModel, new();

        Task<IEnumerable<TModel>> ReadAsync<TModel>(IEnumerable<int> ids) where TModel : class, IModel, new();

        Task<int> UpdateAsync<TModel>(TModel model) where TModel : class, IModel, new();

        Task<TModel> UpdateAsync<TModel>(int id, Action<TModel> updater) where TModel : class, IModel, new();

        Task UpdateAsync<TModel>(IEnumerable<TModel> models) where TModel : class, IModel, new();

        Task<int> DeleteAsync<TModel>(TModel model) where TModel : class, IModel, new();

        Task DeleteAsync<TModel>(IEnumerable<TModel> models) where TModel : class, IModel, new();

        Task ClearAsync<TModel>() where TModel : class, IModel, new();
    }
}