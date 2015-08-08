﻿using NHibernate;
using NHibernate.Linq;
using NHibernate.Persister.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MasDev.Data
{
    public class NHibernateRepository : IRepository
    {
        #region constructor

        internal NHibernateUnitOfWork Uow;

        protected ISession Session { get { return Uow.Session; } }

        public virtual IUnitOfWork UnitOfWork { get { return Uow; } }

        public NHibernateRepository(IUnitOfWork uow)
        {
            Uow = uow as NHibernateUnitOfWork;
            if (Uow == null)
                throw new ArgumentException("uow must be subclass of " + typeof(NHibernateUnitOfWork).FullName);
            Uow.Start();
        }

        #endregion

        public virtual int Create<T>(T model) where T : class, IModel, new()
        {
            var undeletable = model as IUndeletableModel;
            if (undeletable != null)
                undeletable.IsDeleted = false;

            Session.Save(model);
            return model.Id;
        }

        public virtual void Create<T>(IEnumerable<T> models) where T : class, IModel, new()
        {
            foreach (var t in models)
            {
                var undeletable = t as IUndeletableModel;
                if (undeletable != null)
                    undeletable.IsDeleted = false;
                Session.Save(t);
            }
        }

        public virtual T Read<T>(int id) where T : class, IModel, new()
        {
            var obj = Session.Get<T>(id);
            return obj;
        }

        public virtual IEnumerable<T> Read<T>(IEnumerable<int> ids) where T : class, IModel, new()
        {
            var result = new List<T>();
            foreach (var id in ids)
            {
                var t = Session.Get<T>(id);
                if (t == null)
                    continue;
                result.Add(t);
            }

            return result;
        }

        public T ReadTransient<T>(int id) where T : class, IModel, new()
        {
            var obj = Uow.ReadonlySession.Get<T>(id);
            return obj;
        }

        public virtual int Update<T>(T model) where T : class, IModel, new()
        {
            Session.Update(model);
            return model.Id;
        }

        public virtual void Update<T>(IEnumerable<T> models) where T : class, IModel, new()
        {
            foreach (var m in models)
            {
                Update(m);
            }
        }

        public T Update<T>(int id, Action<T> updater) where T : class, IModel, new()
        {
            var model = Read<T>(id);
            if (model == null)
                return null;

            updater(model);
            Update(model);
            return model;
        }

        public virtual int Delete<T>(T model) where T : class, IModel, new()
        {
            var undeletable = model as IUndeletableModel;
            if (undeletable == null)
                Session.Delete(model);
            else
            {
                undeletable.IsDeleted = true;
                Session.Update(undeletable);
            }
            return model.Id;
        }

        public virtual void Delete<T>(IEnumerable<T> models) where T : class, IModel, new()
        {
            foreach (var m in models)
            {
                Delete(m);
            }
        }

        public void Clear<T>() where T : class, IModel, new()
        {
            var metadata = Session.SessionFactory.GetClassMetadata(typeof(T)) as AbstractEntityPersister;
            string table = metadata.TableName;
            string deleteAll = string.Format("DELETE FROM \"{0}\"", table);

            Session.Delete(deleteAll);
        }

        public int CreateOrUpdate<T>(T model) where T : class, IModel, new()
        {
            Session.SaveOrUpdate(model);
            return model.Id;
        }

        public IEnumerable<int> CreateOrUpdate<T>(IEnumerable<T> models) where T : class, IModel, new()
        {
            var ids = new List<int>();
            foreach (var model in models)
                ids.Add(CreateOrUpdate(model));
            return ids;
        }

        public virtual void BeginWork()
        {
            if (IsInTransaction)
                throw new Exception("Nested works are not allowed");
            Uow.Start();
        }

        public virtual void CommitWork()
        {
            if (IsInTransaction)
                Uow.Commit(true);
            else
                throw new Exception("Work not has not started");
        }

        public virtual void RollbackWork()
        {
            if (IsInTransaction)
                Uow.Rollback(true);
            else
                throw new Exception("Work not has not started");
        }

        public bool IsInTransaction { get { return Uow.IsStarted; } }

        public void Lock<T>(T model, LockMode lockMode) where T : class, IModel, new()
        {
            if (!Uow.IsStarted)
                throw new Exception("Cannot lock an unstarted job");

            Uow.Session.Lock(model, ConvertLockMode(lockMode));
        }

        static global::NHibernate.LockMode ConvertLockMode(LockMode lockMode)
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
                    throw new ArgumentException();
            }
        }

        public IQueryable<TModel> QueryFor<TModel>() where TModel : class, IModel, new()
        {
            return Uow.Session.Query<TModel>();
        }
    }
}