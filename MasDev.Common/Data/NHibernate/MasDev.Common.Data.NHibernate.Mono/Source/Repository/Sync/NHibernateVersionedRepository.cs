using System;
using System.Collections.Generic;
using System.Linq;

namespace MasDev.Data
{
    public abstract class NHibernateVersionedRepository<TVersionedModel, TModelVersioning> : NHibernateRepository<TVersionedModel>, IRepository<TVersionedModel, TModelVersioning>
        where TVersionedModel : class, IVersionedModel<TModelVersioning>, new()
        where TModelVersioning : class, IModelVersioning<TVersionedModel>, new()
    {
        public IQueryable<TVersionedModel> UnfilteredQuery { get { return QueryFor<TVersionedModel>(); } }

        public override IQueryable<TVersionedModel> Query { get { return base.Query.Where(model => !model.IsDeleted); } }

        protected NHibernateVersionedRepository(IUnitOfWork uow) : base(uow)
        {
        }

        protected abstract bool ShouldDoVersioning(TVersionedModel storedModel, TVersionedModel newModel);

        public abstract TModelVersioning CreateVersion(TVersionedModel model);

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
            var mods = models.ToArray();
            var versions = new TModelVersioning[mods.Length];

            for (var i = 0; i < mods.Length; i++)
            {
                var model = mods[i];
                model.IsDeleted = false;
                var version = CreateVersion(model);
                CreateOrUpdate(version);
                versions[i] = version;
            }
            for (var i = 0; i < mods.Length; i++)
            {
                var model = mods[i];
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

        public TModel RawUpdate<TModel>(int id, Action<TModel> updater) where TModel : class, IModel, new()
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
    }
}
