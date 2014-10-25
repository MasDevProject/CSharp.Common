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
		int Create (T model, bool saveChanges = false);



		Task<int> CreateAsync (T model, bool saveChanges = false);



		Task<int> CreateOrUpdateAsync (IModel model, bool saveChanges);



		int CreateOrUpdate (IModel model, bool saveChanges);



		T Read (int id);



		Task<T> ReadAsync (int id);



		int Update (T model, bool saveChanges = false);



		Task<int> UpdateAsync (T model, bool saveChanges = false);



		T Update (int id, Action<T> updater, bool updateChanges = false);



		Task<T> UpdateAsync (int id, Action<T> updater, bool updateChanges = false);



		int Delete (T model, bool saveChanges = false);



		Task<int> DeleteAsync (T model, bool saveChanges = false);



		void Create (IEnumerable<T> models, bool saveChanges = false);



		Task CreateAsync (IEnumerable<T> models, bool saveChanges = false);



		IEnumerable<T> Read (IEnumerable<int> ids, bool saveChanges = false);



		Task<IEnumerable<T>> ReadAsync (IEnumerable<int> ids, bool saveChanges = false);



		void Update (IEnumerable<T> models, bool saveChanges = false);



		Task UpdateAsync (IEnumerable<T> models, bool saveChanges = false);



		void Delete (IEnumerable<T> models, bool saveChanges = false);



		Task DeleteAsync (IEnumerable<T> models, bool saveChanges = false);



		IQueryable<T> Query { get; }



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



		int UpdateVersioned (TVersionedModel model, bool saveChanges = false);



		Task<int> UpdateVersionedAsync (TVersionedModel model, bool saveChanges = false);
	}
}
	