using NHibernate;
using System;


namespace MasDev.Data.NHibernate.Providers
{
	public class NHibernateSqlServerSessionFactoryProvider<TMappingProvider> : ISessionFactoryProvider 
        where TMappingProvider : PersistenceMapper, new()
	{
		protected readonly Lazy<ISessionFactory> FactoryLazy;

		public NHibernateSqlServerSessionFactoryProvider (string modelsNamespace, string host, string database, string schema, string username, string password, string context, bool buildSchema = false)
		{
			FactoryLazy = new Lazy<ISessionFactory> (() => {
				var model = NHibernateUtils.CreateMappings<TMappingProvider> (modelsNamespace);
				var factory = NHibernateUtils.BuildSqlServerFactory<TMappingProvider> (host, database, schema, username, password, model, context);
				return factory;
			});
		}

		public virtual ISessionFactory Factory { get { return FactoryLazy.Value; } }
	}
}

