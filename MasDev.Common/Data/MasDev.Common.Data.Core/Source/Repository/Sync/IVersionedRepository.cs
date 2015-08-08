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

        int RawUpdate<TModel>(TModel model) where TModel : IModel;

        TModel RawUpdate<TModel>(int id, Action<TModel> updater) where TModel : IModel;

        void RawUpdate<TModel>(IEnumerable<TModel> models) where TModel : IModel;

        int RawDelete<TModel>(TModel model) where TModel : IModel;

        void RawDelete<TModel>(IEnumerable<TModel> models) where TModel : IModel;
    }
}
