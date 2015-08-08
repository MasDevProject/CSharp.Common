using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MasDev.Data
{
    public interface IAsyncRepository<TVersionedModel, TModelVersioning> : IRepository<TVersionedModel, TModelVersioning>, IAsyncRepository<TVersionedModel>
      where TVersionedModel : class, IVersionedModel<TModelVersioning>, new()
      where TModelVersioning : class, IModelVersioning<TVersionedModel>, new()
    {
        Task<int> RawUpdateAsync<TModel>(TModel model) where TModel : class, IModel, new();

        Task<TModel> RawUpdateAsync<TModel>(int id, Action<TModel> updater) where TModel : class, IModel, new();

        Task RawUpdateAsync<TModel>(IEnumerable<TModel> models) where TModel : class, IModel, new();

        Task<int> RawDeleteAsync<TModel>(TModel model) where TModel : class, IModel, new();

        Task RawDeleteAsync<TModel>(IEnumerable<TModel> models) where TModel : class, IModel, new();
    }
}