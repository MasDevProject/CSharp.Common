using MasDev.Common.Modeling;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using MasDev.Common.Data;


namespace MasDev.Common.Data
{
	public interface IRepository<T> : IDisposable, IModelQueryFactory where T : class, IModel, new()
	{
		int Create (T model);



		Task<int> CreateAsync (T model);



		Task<int> CreateOrUpdateAsync (IModel model);



		int CreateOrUpdate (IModel model);



		T Read (int id);



		Task<T> ReadAsync (int id);



		int Update (T model);



		Task<int> UpdateAsync (T model);



		T Update (int id, Action<T> updater);



		Task<T> UpdateAsync (int id, Action<T> updater);



		int Delete (T model);



		Task<int> DeleteAsync (T model);



		void Create (IEnumerable<T> models);



		Task CreateAsync (IEnumerable<T> models);



		IEnumerable<T> Read (IEnumerable<int> ids);



		Task<IEnumerable<T>> ReadAsync (IEnumerable<int> ids);



		void Update (IEnumerable<T> models);



		Task UpdateAsync (IEnumerable<T> models);



		void Delete (IEnumerable<T> models);



		Task DeleteAsync (IEnumerable<T> models);



		void Clear ();



		Task ClearAsync ();



		IQueryable<T> Query { get; }



		IUnitOfWork UnitOfWork { get; }


		void BeginWork ();



		void CommitWork ();



		void RollbackWork ();



		bool IsInTransaction { get; }



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



		int UpdateVersioned (TVersionedModel model);



		Task<int> UpdateVersionedAsync (TVersionedModel model);



		IQueryable<TVersionedModel> UnfilteredQuery { get; }
	}
}
	