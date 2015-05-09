using System;
using MasDev.Owin.Middlewares;
using MasDev.Owin.Auth;
using MasDev.Owin.PathMapping;
using Owin;

namespace MasDev.Owin
{
	static class Extensions
	{
		public static void UseAuthorizationMiddleware (this IAppBuilder app, AccessTokenPipeline pipeline, IAccessTokenStore store)
		{
			app.Use<AuthorizationMiddleware> (pipeline, store);
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

