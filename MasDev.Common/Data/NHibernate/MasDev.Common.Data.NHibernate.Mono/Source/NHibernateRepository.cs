using System;
using MasDev.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Linq;
using System.Linq;
using NHibernate.Persister.Entity;
using MasDev.Utils;


namespace MasDev.Data
{
	public class NHibernateRepository<T> : IRepository<T> where T : class, IModel, new()
	{
		internal NHibernateUnitOfWork Uow;

		protected ISession Session { get { return Uow.Session; } }

		public virtual IUnitOfWork UnitOfWork { get { return Uow; } }

		public NHibernateRepository (IUnitOfWork uow)
		{
			Uow = uow as NHibernateUnitOfWork;
			if (Uow == null)
				throw new ArgumentException ("uow must be subclass of NHibernateUnitOfWork");

			Uow.Start ();
		}

		#region Create

		public virtual int Create (T model)
		{
			ThrowIfVersionedModel ();
			var undeletable = model as IUndeletableModel;
			if (undeletable != null)
				undeletable.IsDeleted = false;

			Session.Save (model);
			return model.Id;
		}

		public virtual async Task<int> CreateAsync (T model)
		{
			return await Task.Factory.StartNew (() => Create (model));
		}

		public virtual void Create (IEnumerable<T> models)
		{
			ThrowIfVersionedModel ();
			foreach (var t in models) {
				var undeletable = t as IUndeletableModel;
				if (undeletable != null)
					undeletable.IsDeleted = false;
				Session.Save (t);
			}

		}

		public virtual async Task CreateAsync (IEnumerable<T> models)
		{
			await Task.Factory.StartNew (() => Create (models));
		}

		#endregion

		#region Read

		public virtual T Read (int id)
		{
			var obj = Session.Get<T> (id);
			return obj;
		}

		public virtual async Task<T> ReadAsync (int id)
		{
			return await Task.Factory.StartNew (() => Read (id));
		}

		public virtual IEnumerable<T> Read (IEnumerable<int> ids)
		{
			var result = new List<T> ();
			foreach (var id in ids) {
				var t = Session.Get<T> (id);
				if (t == null)
					continue;
				result.Add (t);
			}

			return result;
		}

		public virtual async Task<IEnumerable<T>> ReadAsync (IEnumerable<int> ids)
		{
			return await Task.Factory.StartNew (() => Read (ids));
		}

		public TModel ReadonlyModel<TModel> (int id) where TModel : class, IModel, new()
		{
			var obj = Uow.ReadonlySession.Get<TModel> (id);
			return obj;
		}

		public async Task<TModel> ReadonlyModelAsync<TModel> (int id) where TModel : class, IModel, new()
		{
			return await Task.Factory.StartNew (() => ReadonlyModel<TModel> (id));
		}

		#endregion

		#region Update

		public virtual int Update (T model)
		{
			Session.Update (model);
			
			return model.Id;
		}

		public virtual async Task<int> UpdateAsync (T model)
		{
			return await Task.Factory.StartNew (() => Update (model));
		}

		public virtual void Update (IEnumerable<T> models)
		{
			foreach (var m in models) {
				Update (m);
			}

		}

		public virtual async Task UpdateAsync (IEnumerable<T> models)
		{
			await Task.Factory.StartNew (() => Update (models));
		}

		public T Update (int id, Action<T> updater)
		{
			var model = Read (id);
			if (model == null)
				return null;

			updater (model);
			Update (model);
			return model;
		}

		public async Task<T> UpdateAsync (int id, Action<T> updater)
		{
			var model = await ReadAsync (id);
			if (model == null)
				return null;

			updater (model);
			await UpdateAsync (model);
			return model;
		}

		#endregion

		#region RawUpdate

		public virtual int RawUpdate<TModel> (TModel model) where TModel:IModel
		{
			Session.Update (model);

			return model.Id;
		}

		public virtual async Task<int> RawUpdateAsync<TModel> (TModel model) where TModel : IModel
		{
			return await Task.Factory.StartNew (() => RawUpdate (model));
		}

		public virtual void RawUpdate<TModel> (IEnumerable<TModel> models) where TModel : IModel
		{
			foreach (var m in models) {
				RawUpdate (m);
			}

		}

		public virtual async Task RawUpdateAsync<TModel> (IEnumerable<TModel> models) where  TModel : IModel
		{
			await Task.Factory.StartNew (() => RawUpdate (models));
		}

		public TModel RawUpdate<TModel> (int id, Action<TModel> updater) where TModel : IModel
		{
			var model = Session.Get<TModel> (id);
			if (model == null)
				return default(TModel);

			updater (model);
			RawUpdate (model);
			return model;
		}

		public async Task<TModel> RawUpdateAsync<TModel> (int id, Action<TModel> updater) where TModel : IModel
		{
			var model = Session.Get<TModel> (id);
			if (model == null)
				return default(TModel);

			updater (model);
			await RawUpdateAsync (model);
			return model;
		}

		#endregion

		#region Delete

		public virtual int Delete (T model)
		{
			var undeletable = model as IUndeletableModel;
			if (undeletable == null)
				Session.Delete (model);
			else {
				undeletable.IsDeleted = true;
				Session.Update (undeletable);
			}
			return model.Id;
		}

		public virtual async Task<int> DeleteAsync (T model)
		{
			return await Task.Factory.StartNew (() => Delete (model));
		}

		public virtual void Delete (IEnumerable<T> models)
		{
			foreach (var m in models) {
				Delete (m);
			}

		}

		public virtual async Task DeleteAsync (IEnumerable<T> models)
		{
			await Task.Factory.StartNew (() => Delete (models));
		}

		#endregion

		#region RawDelete

		public virtual int RawDelete<TModel> (TModel model) where TModel : IModel
		{
			Session.Delete (model);
			return model.Id;
		}

		public virtual async Task<int> RawDeleteAsync<TModel> (TModel model) where TModel : IModel
		{
			return await Task.Factory.StartNew (() => RawDelete (model));
		}

		public virtual void RawDelete<TModel> (IEnumerable<TModel> models) where TModel : IModel
		{
			foreach (var m in models) {
				RawDelete (m);
			}

		}

		public virtual async Task RawDeleteAsync<TModel> (IEnumerable<TModel> models) where TModel : IModel
		{
			await Task.Factory.StartNew (() => RawDelete (models));
		}

		#endregion

		#region Clear

		public void Clear ()
		{
			var metadata = Session.SessionFactory.GetClassMetadata (typeof(T)) as AbstractEntityPersister;
			string table = metadata.TableName;
			string deleteAll = string.Format ("DELETE FROM \"{0}\"", table);

			Session.Delete (deleteAll);
		}

		public async Task ClearAsync ()
		{
			await Task.Factory.StartNew (Clear);
		}

		#endregion

		#region CreateOrUpdate

		public async Task<int> CreateOrUpdateAsync (IModel model)
		{
			return await Task.Factory.StartNew (() => CreateOrUpdate (model));
		}

		public int CreateOrUpdate (IModel model)
		{
			Session.SaveOrUpdate (model);

			return model.Id;
		}

		public async Task<IEnumerable<int>> CreateOrUpdateAsync (IEnumerable<IModel> models)
		{
			return await Task.Factory.StartNew (() => CreateOrUpdate (models));
		}

		public IEnumerable<int> CreateOrUpdate (IEnumerable<IModel> models)
		{
			var ids = new List<int> ();
			foreach (var model in models)
				ids.Add (CreateOrUpdate (model));
			return ids;
		}

		public virtual IQueryable<TModel> UnfilteredQueryForModel<TModel> () where TModel : IModel
		{
			return Uow.Session.Query<TModel> ();
		}

		#endregion

		#region UnitOfWork

		public virtual void BeginWork ()
		{
			if (IsInTransaction)
				throw new Exception ("Nested works are not allowed");
			Uow.Start ();
		}

		public virtual void CommitWork ()
		{
			if (IsInTransaction)
				Uow.Commit (true);
			else
				throw new Exception ("Work not has not started");
		}

		public virtual void RollbackWork ()
		{
			if (IsInTransaction)
				Uow.Rollback (true);
			else
				throw new Exception ("Work not has not started");
		}

		public bool IsInTransaction { get { return Uow.IsStarted; } }

		public void Lock (T model, LockMode lockMode)
		{
			if (!Uow.IsStarted)
				throw new Exception ("Cannot lock an unstarted job");

			Uow.Session.Lock (model, ConvertLockMode (lockMode));
		}

		static global::NHibernate.LockMode ConvertLockMode (LockMode lockMode)
		{
			switch (lockMode) {
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

		#endregion

		public virtual IQueryable<T> Query { get { return UnfilteredQueryForModel<T> (); } }

		public virtual IQueryable<TModel> QueryForModel<TModel> () where TModel : IUndeletableModel
		{
			return UnfilteredQueryForModel<TModel> ().Where (m => !m.IsDeleted);
		}

		static void ThrowIfVersionedModel ()
		{
			if (typeof(IVersionedModel).IsAssignableFrom (typeof(T)))
				throw new NotSupportedException ("Must reimplement for versioning purposes");
		}
	}


	public abstract class NHibernateRepository<TVersionedModel, TModelVersioning> : NHibernateRepository<TVersionedModel>,  IRepository<TVersionedModel, TModelVersioning>
		where TVersionedModel : class, IVersionedModel<TModelVersioning>, new()
		where TModelVersioning : class, IModelVersioning<TVersionedModel>, new()
	{

		protected NHibernateRepository (IUnitOfWork uow) : base (uow)
		{
		}

		public override int Create (TVersionedModel model)
		{
			var version = CreateVersion (model);
			version.IsDeleted = false;
			CreateOrUpdate (version);
			model.CurrentVersion = version;
			model.IsDeleted = false;
			CreateOrUpdate (model);
			version.Parent = model;
			CreateOrUpdate (version);
			return model.Id;
		}

		public override void Create (IEnumerable<TVersionedModel> m)
		{
			var models = m.ToArray ();
			var versions = new TModelVersioning[models.Length];

			for (var i = 0; i < models.Length; i++) {
				var model = models [i];
				model.IsDeleted = false;
				var version = CreateVersion (model);
				CreateOrUpdate (version);
				versions [i] = version;
			}
			for (var i = 0; i < models.Length; i++) {
				var model = models [i];
				var version = versions [i];
				model.CurrentVersion = version;
				CreateOrUpdate (model);

				version.Parent = model;
				CreateOrUpdate (version);
			}
		}

		public override async Task<int> CreateAsync (TVersionedModel model)
		{
			var version = CreateVersion (model);
			await CreateOrUpdateAsync (version);
			version.IsDeleted = false;
			model.CurrentVersion = version;
			model.IsDeleted = false;
			await CreateOrUpdateAsync (model);
			version.Parent = model;
			await CreateOrUpdateAsync (version);
			return model.Id;
		}

		public override async Task CreateAsync (IEnumerable<TVersionedModel> m)
		{
			var models = m.ToArray ();
			var versions = new TModelVersioning[models.Length];
			for (var i = 0; i < models.Length; i++) {
				var model = models [i];
				model.IsDeleted = false;
				var version = CreateVersion (model);
				await CreateOrUpdateAsync (version);
				versions [i] = version;
			}
			for (var i = 0; i < models.Length; i++) {
				var model = models [i];
				var version = versions [i];
				model.CurrentVersion = version;
				await CreateOrUpdateAsync (model);
				version.Parent = model;
				await CreateOrUpdateAsync (version);
			}
		}

		public override int Update (TVersionedModel model)
		{
			var oldModel = Uow.ReadonlySession.Get<TVersionedModel> (model.Id);
			if (!this.ShouldVersion (oldModel, model))
				return base.Update (model);

			var version = CreateVersion (model);
			version.Parent = model;
			CreateOrUpdate (version);
			model.CurrentVersion = version;
			Lock (model, LockMode.Upgrade);
			CreateOrUpdate (model);
			return model.Id;
		}

		public override async Task<int> UpdateAsync (TVersionedModel model)
		{
			var oldModel = Uow.ReadonlySession.Get<TVersionedModel> (model.Id);
			if (!this.ShouldVersion (oldModel, model))
				return await base.UpdateAsync (model);

			var version = CreateVersion (model);
			version.Parent = model;
			await CreateOrUpdateAsync (version);

			model.CurrentVersion = version;
			Lock (model, LockMode.Upgrade);
			await CreateOrUpdateAsync (model);
			return model.Id;
		}

		bool IRepository<TVersionedModel, TModelVersioning>.ShouldDoVersioning (TVersionedModel storedModel, TVersionedModel newModel)
		{
			Assert.NotNull (newModel);

			if (storedModel == null) // It happens when creating and updating the model in the same IUnitOfWork session
				return false;

			if (newModel.Id != storedModel.Id)
				throw new ArgumentException ("Models id must be the same");

			return ShouldDoVersioning (storedModel, newModel);
		}

		public new IQueryable<TVersionedModel> Query { get { return QueryForModel<TVersionedModel> (); } }

		public virtual IQueryable<TVersionedModel> UnfilteredQuery { get { return UnfilteredQueryForModel<TVersionedModel> (); } }

		protected abstract bool ShouldDoVersioning (TVersionedModel storedModel, TVersionedModel newModel);

		public abstract TModelVersioning CreateVersion (TVersionedModel model);
	}

	static class NHibernateBaseRepositoryExtensions
	{
		public static bool ShouldVersion<TVersionedModel, TModelVersioning> (this NHibernateRepository<TVersionedModel, TModelVersioning> repo, TVersionedModel storedModel, TVersionedModel newModel)
			where TVersionedModel : class, IVersionedModel<TModelVersioning>, new()
			where TModelVersioning : class, IModelVersioning<TVersionedModel>, new()
		{
			return (repo as IRepository<TVersionedModel, TModelVersioning>).ShouldDoVersioning (storedModel, newModel);
		}
	}

}

