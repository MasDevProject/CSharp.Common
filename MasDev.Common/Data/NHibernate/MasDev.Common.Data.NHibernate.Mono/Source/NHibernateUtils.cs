using FluentNHibernate.Automapping;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.SqlCommand;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using MasDev.Reflection;
using System.Linq.Expressions;
using MasDev.Extensions;
using System.Diagnostics;


namespace MasDev.Data.NHibernate
{
    public static class NHibernateUtils
    {
        public static AutoPersistenceModel CreateMappings<TPersistenceMapper>(string modelsNamespace) where TPersistenceMapper : PersistenceMapper, new()
        {
            var assemblies = GetAllAssembliesInWithCurrentNamespace(typeof(TPersistenceMapper), modelsNamespace);
            var model = AutoMap.Assemblies(new PersistenceMapperAutoMapConfiguration<TPersistenceMapper>(modelsNamespace), assemblies);
            return model;
        }

        static Assembly[] GetAllAssembliesInWithCurrentNamespace(Type t, string modelsNamespace)
        {
            var typeAssembly = t.Assembly;
            var assemblies = new HashSet<Assembly> { typeAssembly };
            typeAssembly
                .LoadRefrencedAssemblies()
                .Where(a => a.ContainsNamespace(modelsNamespace, false))
                .ForEach(a => assemblies.Add(a));
            return assemblies.Distinct().ToArray();
        }

        static void BuildSchema(Configuration config, bool buildSchema)
        {
            #region DEBUG
            //config.SetInterceptor (new SqlStatementInterceptor ());
            //var update = new SchemaUpdate (config);
            //update.Execute (true, true);
            #endregion

            config.Properties.Add("use_proxy_validator", "false");
            if (!buildSchema)
                return;
            var update = new SchemaUpdate(config);
            update.Execute(false, true);

        }

        public static ISessionFactory BuildMySqlSessionFactory<TPersistenceMapper>(AutoPersistenceModel model, SessionFactoryCreationOptions opts) where TPersistenceMapper : PersistenceMapper, new()
        {
            var dbConfig = Fluently.Configure();

            if (opts.CacheConfig != null)
                dbConfig = dbConfig.Cache(opts.CacheConfig);

            return dbConfig.Database(MySQLConfiguration.Standard
                    .Dialect<MySQL5Dialect>()
                    .ConnectionString(c => c
                       .Server(opts.Host)
                       .Database(opts.Database)
                       .Username(opts.Username)
                       .Password(opts.Password)))
            .CurrentSessionContext(opts.Context)
            .Mappings(config => config.AutoMappings.Add(model.Conventions.Add<PersistenceMapperConvention<TPersistenceMapper>>()))
            .ExposeConfiguration(config => BuildSchema(config, opts.BuildSchema))
            .BuildSessionFactory();
        }

        public static ISessionFactory BuildSqlServerFactory<TPersistenceMapper>(AutoPersistenceModel model, SessionFactoryCreationOptions<MsSqlConfiguration> opts) where TPersistenceMapper : PersistenceMapper, new()
        {
            var dbConfig = Fluently.Configure();

            if (opts.CacheConfig != null)
                dbConfig = dbConfig.Cache(opts.CacheConfig);

            return dbConfig.Database(opts.AdvancedOptions ?? MsSqlConfiguration.MsSql2008
                            .ConnectionString(c => c
                           .Server(opts.Host)
                           .Database(opts.Database)
                           .Username(opts.Username)
                           .Password(opts.Password))
                        .DefaultSchema(opts.Schema))
                .CurrentSessionContext(opts.Context)
                .Mappings(config => config.AutoMappings.Add(model.Conventions.Add<PersistenceMapperConvention<TPersistenceMapper>>()))
                .ExposeConfiguration(config => BuildSchema(config, opts.BuildSchema))
                .BuildSessionFactory();
        }

        public static bool PropertyInDinamicExpressionMatchesMemberName(dynamic expr, string memberName)
        {
            return ExtractPropertyNameFromProxiedExpression(expr) == memberName;
        }

        public static string ExtractPropertyNameFromProxiedExpression(dynamic expr)
        {
            LambdaExpression e = expr;
            var body = e.Body as UnaryExpression;
            var operand = body != null ? (body.Operand as MemberExpression) : (e.Body as MemberExpression);
            return ExpressionsParser.DynamicParsePropertyName(operand);
        }
    }

    public class SessionFactoryCreationOptions
    {
        public string Host;
        public string Database;
        public string Username;
        public string Password;
        public string Schema;
        public string Context;
        public bool BuildSchema;
        public Action<CacheSettingsBuilder> CacheConfig;


        public SessionFactoryCreationOptions()
        {
            BuildSchema = false;
            Context = "web";
        }
    }

    public class SessionFactoryCreationOptions<T> : SessionFactoryCreationOptions
    {
        public T AdvancedOptions;
    }

    class SqlStatementInterceptor : EmptyInterceptor
    {
        public override SqlString OnPrepareStatement(SqlString sql)
        {
            Debug.WriteLine(sql.ToString());
            return sql;
        }
    }


}

