﻿using MasDev.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;


namespace MasDev.Data
{
	public interface IRepository<T> : IDisposable, IModelQueryFactory where T : class, IModel, new()
	{
		#region Create

		int Create (T model);

		Task<int> CreateAsync (T model);

		void Create (IEnumerable<T> models);

		Task CreateAsync (IEnumerable<T> models);

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

		T Read (int id);

		Task<T> ReadAsync (int id);

		IEnumerable<T> Read (IEnumerable<int> ids);

		Task<IEnumerable<T>> ReadAsync (IEnumerable<int> ids);

		#endregion

		#region Update

		int Update (T model);

		Task<int> UpdateAsync (T model);

		T Update (int id, Action<T> updater);

		Task<T> UpdateAsync (int id, Action<T> updater);

		void Update (IEnumerable<T> models);

		Task UpdateAsync (IEnumerable<T> models);

		#endregion

		#region Delete

		int Delete (T model);

		Task<int> DeleteAsync (T model);

		void Delete (IEnumerable<T> models);

		Task DeleteAsync (IEnumerable<T> models);

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

		IQueryable<T> Query { get; }

		IUnitOfWork UnitOfWork { get; }

		void Lock (T model, LockMode lockMode);

	}

	public enum LockMode
	{
		Write,
		Upgrade,
		UpgradeNoWait,
		Read,
		None
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
	