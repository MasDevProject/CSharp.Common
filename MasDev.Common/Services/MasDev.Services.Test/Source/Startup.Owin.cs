using Owin;
using MasDev.Services.Middlewares;
using MasDev.Services.Auth;
using MasDev.Patterns.Injection;
using MasDev.Patterns.Injection.SimpleInjector;
using MasDev.Services.Test;
using MasDev.Data.NHibernate;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace MasDev.Services
{
	partial class Startup
	{
		public void Configuration (IAppBuilder builder)
		{
			AppInjector.PerRequestLifestyle = new ExecutionContextScopeLifestyle ();
			var container = new SimpleInjectorContainer ();
			Injector.InitializeWith (container, new TestDependencyConfigurator ());

			var dbSession = Injector.Resolve<ISessionFactoryProvider> ();
			dbSession.Connect ();

			#if DEBUG
			builder.UseDiagnosticMiddleware ();
			#endif

			builder.UseSimpleInjectorMiddleware (container);
			builder.UseUrlRewriteMiddleware (new UrlRewriteRules ());
			builder.UseRedirectMiddleware (new RedirectRules ());
			builder.UseAuthorizationMiddleware (new AuthorizationRules (), new DefaultAccessTokenPipeline ("pwd"), Injector.Resolve<IAccessTokenStore>);
			builder.UseUnitOfWorkHandlerMiddleware ();


			builder.UseWebApi (ConfigureWebApi ());
		}
	}
}

