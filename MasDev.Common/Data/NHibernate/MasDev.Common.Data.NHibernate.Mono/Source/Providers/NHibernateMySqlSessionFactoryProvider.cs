using NHibernate;
using System;


namespace MasDev.Data.NHibernate.Providers
{
	public class NHibernateMySqlSessionFactoryProvider<TMappingProvider> : ISessionFactoryProvider 
        where TMappingProvider : PersistenceMapper, new()
	{
		protected readonly Lazy<ISessionFactory> FactoryLazy;

		public NHibernateMySqlSessionFactoryProvider (string modelsNamespace, string host, string database, string username, string password, string context, bool buildSchema = false)
		{
			FactoryLazy = new Lazy<ISessionFactory> (() => {
				var model = NHibernateUtils.CreateMappings<TMappingProvider> (modelsNamespace);
				return NHibernateUtils.BuildMySqlSessionFactory<TMappingProvider> (host, database, username, password, model, context);
			});
		}

		public virtual ISessionFactory Factory{ get { return FactoryLazy.Value; } }
	}
}

