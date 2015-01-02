using NHibernate;
using System;


namespace MasDev.Data.NHibernate.Providers
{
	public class NHibernateMySqlSessionFactoryProvider<TMappingProvider> : ISessionFactoryProvider 
        where TMappingProvider : PersistenceMapper, new()
	{
		readonly Lazy<ISessionFactory> _lazyFactory;



		public NHibernateMySqlSessionFactoryProvider (string modelsNamespace, string host, string database, string username, string password, string context)
		{
			_lazyFactory = new Lazy<ISessionFactory> (() => {
				var model = NHibernateUtils.CreateMappings<TMappingProvider> (modelsNamespace);
				return NHibernateUtils.BuildMySqlSessionFactory<TMappingProvider> (host, database, username, password, model, context);
			});
		}



		public ISessionFactory Factory{ get { return _lazyFactory.Value; } }
	}
}

