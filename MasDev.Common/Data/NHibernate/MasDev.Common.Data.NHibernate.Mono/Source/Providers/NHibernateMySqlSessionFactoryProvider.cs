using NHibernate;

namespace MasDev.Data.NHibernate.Providers
{
	public class NHibernateMySqlSessionFactoryProvider<TMappingProvider> : ISessionFactoryProvider 
        where TMappingProvider : PersistenceMapper, new()
	{

		ISessionFactory _factory;
		readonly string _modelsNamespace;
		readonly SessionFactoryCreationOptions _opts;

		public NHibernateMySqlSessionFactoryProvider (string modelsNamespace, SessionFactoryCreationOptions opts)
		{
            _modelsNamespace = modelsNamespace;
            _opts = opts;
		}

		public virtual ISessionFactory Factory{ get { return _factory; } }

		public virtual void Connect ()
		{
			var model = NHibernateUtils.CreateMappings<TMappingProvider> (_modelsNamespace);
			_factory = NHibernateUtils.BuildMySqlSessionFactory<TMappingProvider> (model, _opts);
		}
	}
}

