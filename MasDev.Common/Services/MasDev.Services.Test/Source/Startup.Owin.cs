using Owin;
using MasDev.Services.Middlewares;
using MasDev.Services.Auth;
using MasDev.Patterns.Injection;
using MasDev.Patterns.Injection.SimpleInjector;
using MasDev.Services.Test;
using MasDev.Data.NHibernate;
using SimpleInjector.Extensions.ExecutionContextScoping;
using Microsoft.Owin.Extensions;

namespace MasDev.Services
{
	partial class Startup
	{
		public void Configuration (IAppBuilder builder)
		{
			App.ConfigFolder = "Configurations";
			App.PerRequestLifestyle = new ExecutionContextScopeLifestyle ();

			var container = new SimpleInjectorContainer ();
			Injector.InitializeWith (container, new TestDependencyConfigurator ());

			var dbSession = Injector.Resolve<ISessionFactoryProvider> ();
			dbSession.Connect ();

			#if DEBUG
			builder.UseDiagnosticMiddleware ();
			#endif

			builder.UseSimpleInjectorMiddleware (container);
			builder.UseUrlRewriteMiddleware (App.ConfigFile ("urlRewrites.json"));
			builder.UseRedirectMiddleware (new RedirectRules ());
			builder.UseStageMarker (PipelineStage.MapHandler);
			builder.UseAuthorizationMiddleware (new AuthorizationRules (), new DefaultAccessTokenPipeline ("pwd"), Injector.Resolve<IAccessTokenStore>);
			builder.UseStageMarker (PipelineStage.Authenticate);
			// STATIC FILES HERE
			builder.UseUnitOfWorkHandlerMiddleware ();
			builder.UseWebApi (ConfigureWebApi ());
		}
	}
}

