using System;
using NHibernate;
using System.Data;
using NHibernate.Metadata;
using System.Collections.Generic;
using NHibernate.Stat;

namespace MasDev.Data.NHibernate.Providers
{
	public abstract class SessionFactoryWrapper : ISessionFactory
	{
		protected readonly ISessionFactory Factory;

		protected SessionFactoryWrapper (ISessionFactory factory)
		{
			Factory = factory;
		}

		ISession ISessionFactory.OpenSession (IDbConnection conn)
		{
			return this.OpenSession (conn);
		}

		ISession ISessionFactory.OpenSession (IInterceptor sessionLocalInterceptor)
		{
			return this.OpenSession (sessionLocalInterceptor);
		}

		ISession ISessionFactory.OpenSession (IDbConnection conn, IInterceptor sessionLocalInterceptor)
		{
			return this.OpenSession (conn, sessionLocalInterceptor);
		}

		ISession ISessionFactory.OpenSession ()
		{
			return this.OpenSession ();
		}

		public IClassMetadata GetClassMetadata (Type persistentClass)
		{
			return Factory.GetClassMetadata (persistentClass);
		}

		public IClassMetadata GetClassMetadata (string entityName)
		{
			return Factory.GetClassMetadata (entityName);
		}

		public ICollectionMetadata GetCollectionMetadata (string roleName)
		{
			return Factory.GetCollectionMetadata (roleName);
		}

		public IDictionary<string, IClassMetadata> GetAllClassMetadata ()
		{
			return Factory.GetAllClassMetadata ();
		}

		public IDictionary<string, ICollectionMetadata> GetAllCollectionMetadata ()
		{
			return Factory.GetAllCollectionMetadata ();
		}

		public void Close ()
		{
			Factory.Close ();
		}

		public void Evict (Type persistentClass)
		{
			Factory.Evict (persistentClass);
		}

		public void Evict (Type persistentClass, object id)
		{
			Factory.Evict (persistentClass, id);
		}

		public void EvictEntity (string entityName)
		{
			Factory.EvictEntity (entityName);
		}

		public void EvictEntity (string entityName, object id)
		{
			Factory.EvictEntity (entityName, id);
		}

		public void EvictCollection (string roleName)
		{
			Factory.EvictCollection (roleName);
		}

		public void EvictCollection (string roleName, object id)
		{
			Factory.EvictCollection (roleName, id);
		}

		public void EvictQueries ()
		{
			Factory.EvictQueries ();
		}

		public void EvictQueries (string cacheRegion)
		{
			Factory.EvictQueries (cacheRegion);
		}

		public IStatelessSession OpenStatelessSession ()
		{
			return Factory.OpenStatelessSession ();
		}

		public IStatelessSession OpenStatelessSession (IDbConnection connection)
		{
			return Factory.OpenStatelessSession (connection);
		}

		public global::NHibernate.Engine.FilterDefinition GetFilterDefinition (string filterName)
		{
			return Factory.GetFilterDefinition (filterName);
		}

		public ISession GetCurrentSession ()
		{
			return Factory.GetCurrentSession ();
		}

		public void Dispose ()
		{
			Factory.Dispose ();
		}

		public IStatistics Statistics { get { return Factory.Statistics; } }

		public bool IsClosed { get { return Factory.IsClosed; } }

		public ICollection<string> DefinedFilterNames { get { return Factory.DefinedFilterNames; } }

		#region Abstracts

		protected abstract ISession OpenSession (IDbConnection conn);

		protected abstract ISession OpenSession (IInterceptor sessionLocalInterceptor);

		protected abstract ISession OpenSession (IDbConnection conn, IInterceptor sessionLocalInterceptor);

		protected abstract  ISession OpenSession ();

		#endregion
	}
}

