using FluentNHibernate.Automapping;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using MasDev.Common.Reflection;
using System.Linq.Expressions;
using MasDev.Common.Extensions;


namespace MasDev.Common.Data.NHibernate
{
	public static class NHibernateUtils
	{
		public static AutoPersistenceModel CreateMappings<TPersistenceMapper> (string modelsNamespace) where TPersistenceMapper : PersistenceMapper, new()
		{
			var assemblies = GetAllAssembliesInWithCurrentNamespace (typeof(TPersistenceMapper), modelsNamespace);
			var model = AutoMap.Assemblies (new PersistenceMapperAutoMapConfiguration<TPersistenceMapper> (modelsNamespace), assemblies);
			return model;
		}



		static Assembly[] GetAllAssembliesInWithCurrentNamespace (Type t, string modelsNamespace)
		{
			var typeAssembly = t.Assembly;
			var assemblies = new HashSet<Assembly> { typeAssembly };
			typeAssembly
				.LoadRefrencedAssemblies ()
				.Where (a => a.ContainsNamespace (modelsNamespace, false))
				.ForEach (a => assemblies.Add (a));
			return assemblies.Distinct ().ToArray ();
		}



		static void BuildSchema (Configuration config)
		{
			#if SQLTRACE
			config.SetInterceptor (new SqlStatementInterceptor ());
			var update = new SchemaUpdate (config);
			update.Execute (true, true);
			#else
			var update = new SchemaUpdate (config);
			update.Execute (false, true);
			#endif
		}



		public static ISessionFactory BuildMySqlSessionFactory<TPersistenceMapper> (string host, string database, string username, string password, AutoPersistenceModel model, string context) where TPersistenceMapper : PersistenceMapper, new()
		{
			return Fluently.Configure ()
				.Database (MySQLConfiguration.Standard
                        .Dialect<MySQL5Dialect> ()
                        .ConnectionString (c => c
						    .Server (host)
						    .Database (database)
						    .Username (username)
						    .Password (password)))
				.CurrentSessionContext (context)
				.Mappings (config => config.AutoMappings.Add (model.Conventions.Add<PersistenceMapperConvention<TPersistenceMapper>> ()))
				.ExposeConfiguration (BuildSchema)
				.BuildSessionFactory ();
		}



		public static ISessionFactory BuildSqlServerFactory<TPersistenceMapper> (string host, string database, string schema, string username, string password, AutoPersistenceModel model, string context) where TPersistenceMapper : PersistenceMapper, new()
		{
			return Fluently.Configure ()
                .Database (MsSqlConfiguration.MsSql7
                        .ConnectionString (c => c
                            .Server (host)
                            .Database (database)
                            .Username (username)
                            .Password (password))
                        .DefaultSchema (schema))
                .CurrentSessionContext (context)
                .Mappings (config => config.AutoMappings.Add (model.Conventions.Add<PersistenceMapperConvention<TPersistenceMapper>> ()))
                .ExposeConfiguration (BuildSchema)
                .BuildSessionFactory ();
		}




		public static bool PropertyInDinamicExpressionMatchesMemberName (dynamic expr, string memberName)
		{
			return ExtractPropertyNameFromProxiedExpression (expr) == memberName;
		}



		public static string ExtractPropertyNameFromProxiedExpression (dynamic expr)
		{
			LambdaExpression e = expr;
			var body = e.Body as UnaryExpression;
			var operand = body != null ? (body.Operand as MemberExpression) : (e.Body as MemberExpression);
			return ExpressionsParser.DynamicParsePropertyName (operand);
		}
	}




	#if SQLTRACE
	class SqlStatementInterceptor : EmptyInterceptor
	{
		public override SqlString OnPrepareStatement (SqlString sql)
		{
			Console.WriteLine (sql.ToString ());
			return sql;
		}
	}
	#endif

}

