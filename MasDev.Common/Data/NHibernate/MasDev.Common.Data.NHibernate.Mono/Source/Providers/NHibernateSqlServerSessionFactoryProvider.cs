using NHibernate;
using System;


namespace MasDev.Data.NHibernate.Providers
{
	public class NHibernateSqlServerSessionFactoryProvider<TMappingProvider> : ISessionFactoryProvider 
        where TMappingProvider : PersistenceMapper, new()
	{
        ISessionFactory _factory;
        readonly string _modelsNamespace;
        readonly string _host;
        readonly string _database;
        readonly string _schema;
        readonly string _username;
        readonly string _password;
        readonly string _context;
        readonly bool _buildSchema;

        public NHibernateSqlServerSessionFactoryProvider (string modelsNamespace, string host, string database, string schema, string username, string password, string context, bool buildSchema = false)
		{
            _modelsNamespace = modelsNamespace;
            _host = host;
            _database = database;
            _schema = schema;
            _username = username;
            _password = password;
            _context = context;
            _buildSchema = buildSchema;
		}

		public virtual ISessionFactory Factory { get { return _factory; } }

		public void Connect ()
		{
            var model = NHibernateUtils.CreateMappings<TMappingProvider>(_modelsNamespace);
            var factory = NHibernateUtils.BuildSqlServerFactory<TMappingProvider>(_host, _database, _schema, _username, _password, model, _context, _buildSchema);
        }
	}
}

