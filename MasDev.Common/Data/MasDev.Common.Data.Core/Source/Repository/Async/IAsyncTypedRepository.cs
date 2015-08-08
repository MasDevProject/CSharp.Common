using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MasDev.Data
{
    public interface IAsyncRepository<TModel> : IRepository<TModel>, IAsyncRepository where TModel : class, IModel, new()
    {
        Task<int> CreateAsync(TModel model);

        Task CreateAsync(IEnumerable<TModel> models);

        Task<int> CreateOrUpdateAsync(TModel model);

        Task<IEnumerable<int>> CreateOrUpdateAsync(IEnumerable<TModel> models);

        Task<IEnumerable<TModel>> ReadAsync(IEnumerable<int> ids);

        Task<TModel> ReadTransientAsync(int id);

        Task<TModel> ReadAsync(int id);

        Task<int> UpdateAsync(TModel model);

        Task UpdateAsync(IEnumerable<TModel> models);

        Task<TModel> UpdateAsync(int id, Action<TModel> updater);
    
        Task<int> DeleteAsync(TModel model);

        Task DeleteAsync(IEnumerable<TModel> models);

        Task ClearAsync();
    }
}
