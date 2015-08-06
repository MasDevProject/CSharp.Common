using System;
using System.Collections.Generic;
using System.Linq;

namespace MasDev.Data
{
    public interface IRepository
    {
        int Create<TModel>(TModel model) where TModel : class, IModel, new();

        void Create<TModel>(IEnumerable<TModel> models) where TModel : class, IModel, new();

        int CreateOrUpdate<TModel>(TModel model) where TModel : class, IModel, new();

        IEnumerable<int> CreateOrUpdate<TModel>(IEnumerable<TModel> models) where TModel : class, IModel, new();

        TModel ReadTransient<TModel>(int id) where TModel : class, IModel, new();

        TModel Read<TModel>(int id) where TModel : class, IModel, new();
   
        IEnumerable<TModel> Read<TModel>(IEnumerable<int> ids) where TModel : class, IModel, new();

        int Update<TModel>(TModel model) where TModel : class, IModel, new();

        TModel Update<TModel>(int id, Action<TModel> updater) where TModel : class, IModel, new();

        void Update<TModel>(IEnumerable<TModel> models) where TModel : class, IModel, new();

        int Delete<TModel>(TModel model) where TModel : class, IModel, new();

        void Delete<TModel>(IEnumerable<TModel> models) where TModel : class, IModel, new();

        void Clear<TModel>() where TModel : class, IModel, new();

        IQueryable<TModel> QueryFor<TModel>() where TModel : class, IModel, new();

        void Lock<TModel>(TModel model, LockMode lockMode) where TModel : class, IModel, new();

        #region UnitOfWork

        void BeginWork();

        void CommitWork();

        void RollbackWork();

        bool IsInTransaction { get; }
    
        IUnitOfWork UnitOfWork { get; }

        #endregion

    }
}
