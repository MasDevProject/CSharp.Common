using NHibernate;


namespace MasDev.Data.NHibernate.Providers
{
	public class NHibernateMySqlSessionFactoryProvider<TMappingProvider> : ISessionFactoryProvider 
        where TMappingProvider : PersistenceMapper, new()
	{

		ISessionFactory _factory;
		readonly string _modelsNamespace;
		readonly string _host;
		readonly string _database;
		readonly string _username;
		readonly string _password;
		readonly string _context;
		readonly bool _buildSchema;

		public NHibernateMySqlSessionFactoryProvider (string modelsNamespace, string host, string database, string username, string password, string context, bool buildSchema = false)
		{
			_modelsNamespace = modelsNamespace;
			_host = host;
			_database = database;
			_username = username;
			_password = password;
			_context = context;
			_buildSchema = buildSchema;
		}

		public virtual ISessionFactory Factory{ get { return _factory; } }

		public virtual void Connect ()
		{
			var model = NHibernateUtils.CreateMappings<TMappingProvider> (_modelsNamespace);
			_factory = NHibernateUtils.BuildMySqlSessionFactory<TMappingProvider> (_host, _database, _username, _password, model, _context, _buildSchema);
		}
	}
}

