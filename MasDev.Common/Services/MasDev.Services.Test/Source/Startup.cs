﻿using Owin;
using MasDev.Services.Middlewares;
using MasDev.Services.Auth;
using MasDev.Patterns.Injection;
using MasDev.Patterns.Injection.SimpleInjector;
using MasDev.Services.Test;

namespace MasDev.Services
{
	class Startup
	{
		public void Configuration (IAppBuilder builder)
		{
			Injector.InitializeWith (new SimpleInjectorContainer (), new TestDependencyConfigurator ());
			builder.UseSimpleInjectorMiddleware ();
			builder.UseUrlRewriteMiddleware (new UrlRewriteRules ());
			builder.UseRedirectMiddleware (new RedirectRules ());
			builder.UseAuthorizationMiddleware (new AuthorizationRules (), new DefaultAccessTokenPipeline ("pwd"), Injector.Resolve<IAccessTokenStore>);
		}
	}
}

