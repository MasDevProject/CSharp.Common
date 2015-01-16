using NHibernate;
using System;


namespace MasDev.Data.NHibernate.Providers
{
	public class NHibernateSqlServerSessionFactoryProvider<TMappingProvider> : ISessionFactoryProvider 
        where TMappingProvider : PersistenceMapper, new()
	{
		readonly Lazy<ISessionFactory> _lazyFactory;



		public NHibernateSqlServerSessionFactoryProvider (string modelsNamespace, string host, string database, string schema, string username, string password, string context)
		{
			_lazyFactory = new Lazy<ISessionFactory> (() => {
				var model = NHibernateUtils.CreateMappings<TMappingProvider> (modelsNamespace);
				var factory = NHibernateUtils.BuildSqlServerFactory<TMappingProvider> (host, database, schema, username, password, model, context);
				return factory;
			});
		}



		public ISessionFactory Factory {
			get {
				return _lazyFactory.Value;
			}
		}
	}
}

