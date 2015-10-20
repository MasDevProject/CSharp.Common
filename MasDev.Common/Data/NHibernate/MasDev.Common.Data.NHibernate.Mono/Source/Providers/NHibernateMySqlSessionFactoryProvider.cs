using NHibernate;

namespace MasDev.Data.NHibernate.Providers
{
	public class NHibernateMySqlSessionFactoryProvider: ISessionFactoryProvider 
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
			_factory = NHibernateUtils.BuildMySqlSessionFactory (_opts);
		}
	}
}

