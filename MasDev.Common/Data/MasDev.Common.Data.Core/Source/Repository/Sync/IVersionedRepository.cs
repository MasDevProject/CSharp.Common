using System;
using System.Collections.Generic;
using System.Linq;

namespace MasDev.Data
{
    public interface IRepository<TVersionedModel, TModelVersioning> : IRepository<TVersionedModel>
      where TVersionedModel : class, IVersionedModel<TModelVersioning>, new()
      where TModelVersioning : class, IModelVersioning<TVersionedModel>, new()
    {
        TModelVersioning CreateVersion(TVersionedModel model);

        IQueryable<TVersionedModel> UnfilteredQuery { get; }

        bool ShouldDoVersioning(TVersionedModel storedModel, TVersionedModel newModel);

        int RawUpdate<TModel>(TModel model) where TModel : class, IModel, new();

        TModel RawUpdate<TModel>(int id, Action<TModel> updater) where TModel : class, IModel, new();

        void RawUpdate<TModel>(IEnumerable<TModel> models) where TModel : class, IModel, new();

        int RawDelete<TModel>(TModel model) where TModel : class, IModel, new();

        void RawDelete<TModel>(IEnumerable<TModel> models) where TModel : class, IModel, new();
    }
}
