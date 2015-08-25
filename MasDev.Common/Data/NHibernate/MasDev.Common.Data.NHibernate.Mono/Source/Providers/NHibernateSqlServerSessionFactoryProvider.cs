using FluentNHibernate.Cfg.Db;
using NHibernate;


namespace MasDev.Data.NHibernate.Providers
{
	public class NHibernateSqlServerSessionFactoryProvider<TMappingProvider> : ISessionFactoryProvider 
        where TMappingProvider : PersistenceMapper, new()
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
            var model = NHibernateUtils.CreateMappings<TMappingProvider>(_modelsNamespace);
            _factory = NHibernateUtils.BuildSqlServerFactory<TMappingProvider>(model, _opts);
        }
	}
}

