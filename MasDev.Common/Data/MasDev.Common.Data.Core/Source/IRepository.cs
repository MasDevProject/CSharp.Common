using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;


namespace MasDev.Data
{
	public enum LockMode
	{
		Write,
		Upgrade,
		UpgradeNoWait,
		Read,
		None
	}

	public interface IRepository<TModel> : IModelQueryFactory where TModel : class, IModel, new()
	{
		#region Create

		int Create (TModel model);

		Task<int> CreateAsync (TModel model);

		void Create (IEnumerable<TModel> models);

		Task CreateAsync (IEnumerable<TModel> models);

		#endregion

		#region CreateOrUpdate

		Task<int> CreateOrUpdateAsync (IModel model);

		int CreateOrUpdate (IModel model);

		Task<IEnumerable<int>> CreateOrUpdateAsync (IEnumerable<IModel> models);

		IEnumerable<int> CreateOrUpdate (IEnumerable<IModel> models);

		#endregion

		#region Read

		TModel ReadonlyModel<TModel> (int id) where TModel : class, IModel, new();

		Task<TModel> ReadonlyModelAsync<TModel> (int id) where TModel : class, IModel, new();

		TModel Read (int id);

		Task<TModel> ReadAsync (int id);

		IEnumerable<TModel> Read (IEnumerable<int> ids);

		Task<IEnumerable<TModel>> ReadAsync (IEnumerable<int> ids);

		#endregion

		#region Update

		int Update (TModel model);

		Task<int> UpdateAsync (TModel model);

		TModel Update (int id, Action<TModel> updater);

		Task<TModel> UpdateAsync (int id, Action<TModel> updater);

		void Update (IEnumerable<TModel> models);

		Task UpdateAsync (IEnumerable<TModel> models);

		#endregion

		#region RawUpdate

		int RawUpdate<TModel> (TModel model) where TModel : IModel;

		Task<int> RawUpdateAsync<TModel> (TModel model) where TModel : IModel;

		TModel RawUpdate<TModel> (int id, Action<TModel> updater) where TModel : IModel;

		Task<TModel> RawUpdateAsync<TModel> (int id, Action<TModel> updater) where TModel : IModel;

		void RawUpdate<TModel> (IEnumerable<TModel> models) where TModel : IModel;

		Task RawUpdateAsync<TModel> (IEnumerable<TModel> models) where TModel : IModel;

		#endregion

		#region Delete

		int Delete (TModel model);

		Task<int> DeleteAsync (TModel model);

		void Delete (IEnumerable<TModel> models);

		Task DeleteAsync (IEnumerable<TModel> models);

		#endregion

		#region RawDelete

		int RawDelete<TModel> (TModel model) where TModel : IModel;

		Task<int> RawDeleteAsync<TModel> (TModel model) where TModel : IModel;

		void RawDelete<TModel> (IEnumerable<TModel> models) where TModel : IModel;

		Task RawDeleteAsync<TModel> (IEnumerable<TModel> models) where TModel : IModel;

		#endregion

		#region Clear

		void Clear ();

		Task ClearAsync ();

		#endregion

		#region UnitOfWork

		void BeginWork ();

		void CommitWork ();

		void RollbackWork ();

		bool IsInTransaction { get; }

		#endregion

		IQueryable<TModel> Query { get; }

		IUnitOfWork UnitOfWork { get; }

		void Lock (TModel model, LockMode lockMode);

	}

	public interface IRepository<TVersionedModel, TModelVersioning> : IRepository<TVersionedModel>
        where TVersionedModel : class, IVersionedModel<TModelVersioning>, new()
        where TModelVersioning : class, IModelVersioning<TVersionedModel>, new()
	{
		TModelVersioning CreateVersion (TVersionedModel model);

		IQueryable<TVersionedModel> UnfilteredQuery { get; }

		bool ShouldDoVersioning (TVersionedModel storedModel, TVersionedModel newModel);
	}
}
