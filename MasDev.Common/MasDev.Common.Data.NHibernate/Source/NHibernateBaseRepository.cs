using System;
using MasDev.Common.Data;
using MasDev.Common.Modeling;
using System.Threading.Tasks;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Linq;
using System.Linq;


namespace MasDev.Common.Data.NHibernate
{
	public class NHibernateBaseRepository<T> : IRepository<T> where T : class, IModel, new()
	{

		public NHibernateBaseRepository (IUnitOfWork uow)
		{
			_uow = uow as NHibernateUnitOfWork;
			if (_uow == null)
				throw new ArgumentException ("uow must be subclass of NHibernateUnitOfWork");

			_uow.Start ();
		}



		NHibernateUnitOfWork _uow;



		ISession Session { get { return _uow.Session; } }



		public virtual int Create (T model, bool saveChanges = false)
		{
			ThrowIfVersionedModel ();
			var id = (int)Session.Save (model);
			SaveChanges (saveChanges);
			return id;
		}



		internal void SaveChanges (bool saveChanges)
		{
			if (!saveChanges)
				return;
			if (!IsInTransaction)
				BeginWork ();
			_uow.Commit (true);
		}



		public virtual async Task<int> CreateAsync (T model, bool saveChanges = false)
		{
			return await Task.Factory.StartNew (() => Create (model, saveChanges));
		}



		public virtual T Read (int id)
		{
			var obj = Session.Get<T> (id);
			return obj;
		}



		public virtual async Task<T> ReadAsync (int id)
		{
			return await Task.Factory.StartNew (() => Read (id));
		}



		public virtual int Update (T model, bool saveChanges = false)
		{
			Session.Update (model);
			SaveChanges (saveChanges);
			return model.Id;
		}



		public virtual async Task<int> UpdateAsync (T model, bool saveChanges = false)
		{
			return await Task.Factory.StartNew (() => Update (model, saveChanges));
		}



		public virtual int Delete (T model, bool saveChanges = false)
		{
			var undeletable = model as IUndeletableModel;
			if (undeletable == null)
				Session.Delete (model);
			else
			{
				undeletable.IsEnabled = false;
				Session.Update (undeletable);
			}
			SaveChanges (saveChanges);
			return model.Id;
		}



		public virtual async Task<int> DeleteAsync (T model, bool saveChanges = false)
		{
			return await Task.Factory.StartNew (() => Delete (model, saveChanges));
		}



		public virtual void Create (IEnumerable<T> models, bool saveChanges = false)
		{
			ThrowIfVersionedModel ();
			foreach (var t in models)
			{
				Session.Save (t);
			}
			SaveChanges (saveChanges);
		}



		public virtual async Task CreateAsync (IEnumerable<T> models, bool saveChanges = false)
		{
			await Task.Factory.StartNew (() => Create (models, saveChanges));
		}



		public virtual IEnumerable<T> Read (IEnumerable<int> ids, bool saveChanges = false)
		{
			var result = new List<T> ();
			foreach (var id in ids)
			{
				var t = Session.Get<T> (id);
				if (t == null)
					continue;
				result.Add (t);
			}
			SaveChanges (saveChanges);
			return result;
		}



		public virtual async Task<IEnumerable<T>> ReadAsync (IEnumerable<int> ids, bool saveChanges = false)
		{
			return await Task.Factory.StartNew (() => Read (ids, saveChanges));
		}



		public virtual void Update (IEnumerable<T> models, bool saveChanges = false)
		{
			foreach (var m in models)
			{
				Session.Update (m);
			}
			SaveChanges (saveChanges);
		}



		public virtual async Task UpdateAsync (IEnumerable<T> models, bool saveChanges = false)
		{
			await Task.Factory.StartNew (() => Update (models, saveChanges));
		}



		public virtual void Delete (IEnumerable<T> models, bool saveChanges = false)
		{
			foreach (var m in models)
			{
				Session.Delete (m);
			}
			SaveChanges (saveChanges);
		}



		public virtual async Task DeleteAsync (IEnumerable<T> models, bool saveChanges = false)
		{
			await Task.Factory.StartNew (() => Delete (models, saveChanges));
		}



		public T Update (int id, Action<T> updater, bool saveChanges = false)
		{
			var model = Read (id);
			if (model == null)
				return null;

			updater (model);
			Update (model, saveChanges);
			return model;
		}



		public async Task<T> UpdateAsync (int id, Action<T> updater, bool saveChanges = false)
		{
			var model = await ReadAsync (id);
			if (model == null)
				return null;

			updater (model);
			await UpdateAsync (model, saveChanges);
			return model;
		}



		public virtual void Dispose ()
		{
			_uow.Dispose ();
		}



		public IQueryable<T> Query { get { return _uow.Session.Query<T> (); } }



		public virtual void BeginWork ()
		{
			if (IsInTransaction)
				throw new Exception ("Nested works are not allowed");
			_uow.Start ();
		}



		public virtual void CommitWork ()
		{
			if (IsInTransaction)
				_uow.Commit ();
			else
				throw new Exception ("Work not has not started");
		}



		public virtual void RollbackWork ()
		{
			if (IsInTransaction)
				_uow.Rollback ();
			else
				throw new Exception ("Work not has not started");
		}



		public bool IsInTransaction { get { return _uow.IsStarted (); } }



		public void Lock (T model, LockMode lockMode)
		{
			if (!_uow.IsStarted ())
				throw new Exception ("Cannot lock an unstarted job");

			_uow.Session.Lock (model, ConvertLockMode (lockMode));
		}



		static global::NHibernate.LockMode ConvertLockMode (LockMode lockMode)
		{
			switch (lockMode)
			{
			case LockMode.None:
				return global::NHibernate.LockMode.None;
			case LockMode.Read:
				return global::NHibernate.LockMode.Read;
			case LockMode.Upgrade:
				return global::NHibernate.LockMode.Upgrade;
			case LockMode.UpgradeNoWait:
				return global::NHibernate.LockMode.UpgradeNoWait;
			case LockMode.Write:
				return global::NHibernate.LockMode.Write;
			default:
				throw new ArgumentException ();
			}
		}



		public async Task<int> CreateOrUpdateAsync (IModel model, bool saveChanges)
		{
			return await Task.Factory.StartNew (() => CreateOrUpdate (model, saveChanges));
		}



		public int CreateOrUpdate (IModel model, bool saveChanges)
		{
			int id = model.Id;
			if (model.Id == 0)
				id = (int)Session.Save (model);
			else
				Session.Update (model);

			SaveChanges (saveChanges);
			return id;
		}



		public IQueryable<TModel> QueryForModel<TModel> () where TModel : IModel
		{
			return _uow.Session.Query<TModel> ();
		}



		static void ThrowIfVersionedModel ()
		{
			if (typeof(IVersionedModel).IsAssignableFrom (typeof(T)))
				throw new NotSupportedException ("Must reimplement for versioning purposes");
		}
	}





	public abstract class NHibernateBaseRepository<TVersionedModel, TModelVersioning> : NHibernateBaseRepository<TVersionedModel>,  IRepository<TVersionedModel, TModelVersioning>
		where TVersionedModel : class, IVersionedModel<TModelVersioning>, new()
		where TModelVersioning : class, IModelVersioning<TVersionedModel>, new()
	{

		protected NHibernateBaseRepository (IUnitOfWork uow) : base (uow)
		{

		}



		public override async Task<int> CreateAsync (TVersionedModel model, bool saveChanges = false)
		{
			var version = CreateVersion (model);
			await CreateOrUpdateAsync (version, true);

			model.CurrentVersion = version;
			await CreateOrUpdateAsync (model, saveChanges);

			version.Parent = model;
			await CreateOrUpdateAsync (version, saveChanges);

			return model.Id;
		}



		public override async Task CreateAsync (IEnumerable<TVersionedModel> m, bool saveChanges = false)
		{
			var models = m.ToArray ();
			var versions = new TModelVersioning[models.Length];

			for (var i = 0; i < models.Length; i++)
			{
				var model = models [i];
				var version = CreateVersion (model);
				await CreateOrUpdateAsync (version, false);
				versions [i] = version;
			}
			SaveChanges (true);

			for (var i = 0; i < models.Length; i++)
			{
				var model = models [i];
				var version = versions [i];
				model.CurrentVersion = version;
				await CreateOrUpdateAsync (model, false);

				version.Parent = model;
				await CreateOrUpdateAsync (version, false);
			}
			SaveChanges (saveChanges);
		}




		public virtual int UpdateVersioned (TVersionedModel model, bool saveChanges = false)
		{
			var version = CreateVersion (model);
			version.Parent = model;
			CreateOrUpdate (version, true);

			model.CurrentVersion = version;
			Lock (model, LockMode.Upgrade);
			CreateOrUpdate (model, saveChanges);

			return model.Id;
		}



		public virtual async Task<int> UpdateVersionedAsync (TVersionedModel model, bool saveChanges = false)
		{
			var version = CreateVersion (model);
			version.Parent = model;
			await CreateOrUpdateAsync (version, true);

			model.CurrentVersion = version;
			Lock (model, LockMode.Upgrade);
			await CreateOrUpdateAsync (model, saveChanges);

			return model.Id;
		}



		public abstract TModelVersioning CreateVersion (TVersionedModel model);
	}

}

