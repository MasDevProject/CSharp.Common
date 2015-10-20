using FluentNHibernate.Automapping;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.SqlCommand;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System;
using MasDev.Reflection;
using System.Linq.Expressions;
using System.Diagnostics;


namespace MasDev.Data.NHibernate
{
    public static class NHibernateUtils
    {
        public static ISessionFactory BuildMySqlSessionFactory(SessionFactoryCreationOptions opts)
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
            .Mappings(opts.Mapper.ConfigureMappings)
            .ExposeConfiguration(config => BuildSchema(config, opts.BuildSchema))
            .BuildSessionFactory();
        }

        public static ISessionFactory BuildSqlServerFactory(SessionFactoryCreationOptions opts)
        {
            var factory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                            .ConnectionString(c => c
                           .Server(opts.Host)
                           .Database(opts.Database)
                           .Username(opts.Username)
                           .Password(opts.Password))
                        .DefaultSchema(opts.Schema));

            if (opts.CacheConfig != null)
                factory.Cache(opts.CacheConfig);

            return factory.CurrentSessionContext(opts.Context)
                .Mappings(opts.Mapper.ConfigureMappings)
                .ExposeConfiguration(config => BuildSchema(config, opts.BuildSchema))
                .BuildSessionFactory();
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
        public readonly IDatabaseMapper Mapper;
        public Action<CacheSettingsBuilder> CacheConfig;


        public SessionFactoryCreationOptions(IDatabaseMapper mapper)
        {
            Mapper = mapper;
            BuildSchema = false;
            Context = "web";
        }
    }

    public class SessionFactoryCreationOptions<T> : SessionFactoryCreationOptions
    {
        public T AdvancedOptions;

        public SessionFactoryCreationOptions(IDatabaseMapper mapper): base(mapper)
        {

        }
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

