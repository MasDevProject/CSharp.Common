using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MasDev.Data
{
    public interface IAsyncRepository<TVersionedModel, TModelVersioning> : IRepository<TVersionedModel, TModelVersioning>, IAsyncRepository<TVersionedModel>
      where TVersionedModel : class, IVersionedModel<TModelVersioning>, new()
      where TModelVersioning : class, IModelVersioning<TVersionedModel>, new()
    {
        Task<int> RawUpdateAsync<TModel>(TModel model) where TModel : IModel;

        Task<TModel> RawUpdateAsync<TModel>(int id, Action<TModel> updater) where TModel : IModel;

        Task RawUpdateAsync<TModel>(IEnumerable<TModel> models) where TModel : IModel;

        Task<int> RawDeleteAsync<TModel>(TModel model) where TModel : IModel;

        Task RawDeleteAsync<TModel>(IEnumerable<TModel> models) where TModel : IModel;
    }
}