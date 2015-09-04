using NHibernate;


namespace MasDev.Data.NHibernate.Providers
{
	public class NHibernateSqlServerSessionFactoryProvider : ISessionFactoryProvider 
	{
        ISessionFactory _factory;
        readonly string _modelsNamespace;
        readonly SessionFactoryCreationOptions _opts;


        public NHibernateSqlServerSessionFactoryProvider (string modelsNamespace, SessionFactoryCreationOptions opts)
		{
            _modelsNamespace = modelsNamespace;
            _opts = opts;
		}

		public virtual ISessionFactory Factory { get { return _factory; } }

		public void Connect ()
		{
            _factory = NHibernateUtils.BuildSqlServerFactory(_opts);
        }
	}
}

