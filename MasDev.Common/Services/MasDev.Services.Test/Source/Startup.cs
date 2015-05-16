﻿using Owin;
using MasDev.Services.Middlewares;
using MasDev.Services.Auth;
using MasDev.Patterns.Injection;
using MasDev.Patterns.Injection.SimpleInjector;
using MasDev.Services.Test;
using MasDev.Data.NHibernate;

namespace MasDev.Services
{
	class Startup
	{
		public void Configuration (IAppBuilder builder)
		{
			var container = new SimpleInjectorContainer ();
			Injector.InitializeWith (container, new TestDependencyConfigurator ());
			var dbSession = Injector.Resolve<ISessionFactoryProvider> ();
			dbSession.Connect ();

			builder.UseSimpleInjectorMiddleware (container);
			builder.UseUrlRewriteMiddleware (new UrlRewriteRules ());
			builder.UseRedirectMiddleware (new RedirectRules ());
			builder.UseAuthorizationMiddleware (new AuthorizationRules (), new DefaultAccessTokenPipeline ("pwd"), Injector.Resolve<IAccessTokenStore>);
			builder.UseUnitOfWorkHandlerMiddleware ();
		}
	}
}

