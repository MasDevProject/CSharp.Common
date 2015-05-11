using System;
using Owin;
using MasDev.Common.Owin.Middlewares;
using MasDev.Common.Owin.Auth;
using MasDev.Common.Owin.Rules;

namespace MasDev.Common.Owin
{
	public static class Extensions
	{
		public static void UseAuthorizationMiddleware (this IAppBuilder app, AuthorizationRules rules, AccessTokenPipeline pipeline, Func<IAccessTokenStore> storeFactory)
		{
			app.Use<AuthorizationMiddleware> (rules, pipeline, storeFactory);
		}

		public static void UseRedirectMiddleware (this IAppBuilder builder, PathMappingRules rules)
		{
			builder.Use<RedirectMiddleware> (rules);
		}

		public static void UseUrlRewriteMiddleware (this IAppBuilder builder, PathMappingRules rules)
		{
			builder.Use<UrlRewriteMiddleware> (rules);
		}

		public static void UseSimpleInjectorMiddleware (this IAppBuilder builder)
		{
			builder.Use<SimpleInjectorMiddleware> ();
		}

		public static void ThrowIfNull (this object obj, string name)
		{
			if (obj == null)
				throw new ArgumentNullException (name);
		}
	}
}

