using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasDev.Data
{
    public abstract class NHibernateAsyncVersionedRepository<TVersionedModel, TModelVersioning> : NHibernateAsyncRepository<TVersionedModel>, IAsyncRepository<TVersionedModel, TModelVersioning>
        where TVersionedModel : class, IVersionedModel<TModelVersioning>, new()
        where TModelVersioning : class, IModelVersioning<TVersionedModel>, new()
    {
        public IQueryable<TVersionedModel> UnfilteredQuery { get { return QueryFor<TVersionedModel>(); } }

        public override IQueryable<TVersionedModel> Query { get { return base.Query.Where(model => !model.IsDeleted); } }

        protected NHibernateAsyncVersionedRepository(IUnitOfWork uow) : base(uow)
        {
        }

        protected abstract bool ShouldDoVersioning(TVersionedModel storedModel, TVersionedModel newModel);

        public abstract TModelVersioning CreateVersion(TVersionedModel model);

        public override async Task<int> CreateAsync(TVersionedModel model)
        {
            var version = CreateVersion(model);
            await CreateOrUpdateAsync(version);
            version.IsDeleted = false;
            model.CurrentVersion = version;
            model.IsDeleted = false;
            await CreateOrUpdateAsync(model);
            version.Parent = model;
            await CreateOrUpdateAsync(version);
            return model.Id;
        }

        public override async Task CreateAsync(IEnumerable<TVersionedModel> models)
        {
            var m = models.ToArray();
            var versions = new TModelVersioning[m.Length];
            for (var i = 0; i < m.Length; i++)
            {
                var model = m[i];
                model.IsDeleted = false;
                var version = CreateVersion(model);
                await CreateOrUpdateAsync(version);
                versions[i] = version;
            }
            for (var i = 0; i < m.Length; i++)
            {
                var model = m[i];
                var version = versions[i];
                model.CurrentVersion = version;
                await CreateOrUpdateAsync(model);
                version.Parent = model;
                await CreateOrUpdateAsync(version);
            }
        }

        public override async Task<int> UpdateAsync(TVersionedModel model)
        {
            var oldModel = Uow.ReadonlySession.Get<TVersionedModel>(model.Id);
            if (oldModel == null || !ShouldDoVersioning(oldModel, model))
                return await base.UpdateAsync(model);

            var version = CreateVersion(model);
            version.Parent = model;
            await CreateOrUpdateAsync(version);

            model.CurrentVersion = version;
            Lock(model, LockMode.Upgrade);
            await CreateOrUpdateAsync(model);
            return model.Id;
        }

        public override int Create(TVersionedModel model)
        {
            var version = CreateVersion(model);
            version.IsDeleted = false;
            CreateOrUpdate(version);
            model.CurrentVersion = version;
            model.IsDeleted = false;
            CreateOrUpdate(model);
            version.Parent = model;
            CreateOrUpdate(version);
            return model.Id;
        }

        public override void Create(IEnumerable<TVersionedModel> models)
        {
            var mod = models.ToArray();
            var versions = new TModelVersioning[mod.Length];

            for (var i = 0; i < mod.Length; i++)
            {
                var model = mod[i];
                model.IsDeleted = false;
                var version = CreateVersion(model);
                CreateOrUpdate(version);
                versions[i] = version;
            }
            for (var i = 0; i < mod.Length; i++)
            {
                var model = mod[i];
                var version = versions[i];
                model.CurrentVersion = version;
                CreateOrUpdate(model);

                version.Parent = model;
                CreateOrUpdate(version);
            }
        }

        public override int Update(TVersionedModel model)
        {
            var oldModel = Uow.ReadonlySession.Get<TVersionedModel>(model.Id);
            if (oldModel == null || !ShouldDoVersioning(oldModel, model))
                return base.Update(model);

            var version = CreateVersion(model);
            version.Parent = model;
            CreateOrUpdate(version);
            model.CurrentVersion = version;
            Lock(model, LockMode.Upgrade);
            CreateOrUpdate(model);
            return model.Id;
        }

        bool IRepository<TVersionedModel, TModelVersioning>.ShouldDoVersioning(TVersionedModel storedModel, TVersionedModel newModel)
        {
            return ShouldDoVersioning(storedModel, newModel);
        }

        public virtual int RawUpdate<TModel>(TModel model) where TModel : class, IModel, new()
        {
            Session.Update(model);
            return model.Id;
        }

        public virtual void RawUpdate<TModel>(IEnumerable<TModel> models) where TModel : class, IModel, new()
        {
            foreach (var m in models)
            {
                RawUpdate(m);
            }
        }

        public virtual TModel RawUpdate<TModel>(int id, Action<TModel> updater) where TModel : class, IModel, new()
        {
            var model = Session.Get<TModel>(id);
            if (model == null)
                return default(TModel);

            updater(model);
            RawUpdate(model);
            return model;
        }

        public virtual int RawDelete<TModel>(TModel model) where TModel : class, IModel, new()
        {
            Session.Delete(model);
            return model.Id;
        }

        public virtual void RawDelete<TModel>(IEnumerable<TModel> models) where TModel : class, IModel, new()
        {
            foreach (var m in models)
            {
                RawDelete(m);
            }
        }

        public virtual async Task<int> RawDeleteAsync<TModel>(TModel model) where TModel : class, IModel, new()
        {
            return await Task.Factory.StartNew(() => RawDelete(model));
        }

        public virtual async Task RawDeleteAsync<TModel>(IEnumerable<TModel> models) where TModel : class, IModel, new()
        {
            await Task.Factory.StartNew(() => RawDelete(models));
        }

        public virtual async Task<int> RawUpdateAsync<TModel>(TModel model) where TModel : class, IModel, new()
        {
            return await Task.Factory.StartNew(() => RawUpdate(model));
        }

        public virtual async Task RawUpdateAsync<TModel>(IEnumerable<TModel> models) where TModel : class, IModel, new()
        {
            await Task.Factory.StartNew(() => RawUpdate(models));
        }

        public virtual async Task<TModel> RawUpdateAsync<TModel>(int id, Action<TModel> updater) where TModel : class, IModel, new()
        {
            var model = Session.Get<TModel>(id);
            if (model == null)
                return default(TModel);

            updater(model);
            await RawUpdateAsync(model);
            return model;
        }
    }
}
