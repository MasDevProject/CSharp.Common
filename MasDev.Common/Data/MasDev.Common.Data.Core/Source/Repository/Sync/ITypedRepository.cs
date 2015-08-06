using System;
using System.Collections.Generic;
using System.Linq;

namespace MasDev.Data
{
    public interface IRepository<TModel> : IRepository where TModel : class, IModel, new()
    {

        int Create(TModel model);

        void Create(IEnumerable<TModel> models);

        int CreateOrUpdate(TModel model);

        IEnumerable<int> CreateOrUpdate(IEnumerable<TModel> models);

        TModel ReadTransient(int id);

        TModel Read(int id);

        IEnumerable<TModel> Read(IEnumerable<int> ids);

        int Update(TModel model);

        TModel Update(int id, Action<TModel> updater);

        void Update(IEnumerable<TModel> models);

        int Delete(TModel model);

        void Delete(IEnumerable<TModel> models);

        void Clear();

        void Lock(TModel model, LockMode lockMode);

        IQueryable<TModel> Query { get; }
    }

}
