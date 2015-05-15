﻿using System;
using Owin;
using MasDev.Services.Auth;

namespace MasDev.Services.Middlewares
{
	public static class Extensions
	{
		public static void UseAuthorizationMiddleware (this IAppBuilder app, BaseAuthorizationRules rules, AccessTokenPipeline pipeline, Func<IAccessTokenStore> storeFactory)
		{
			app.Use<AuthorizationMiddleware> (rules, pipeline, storeFactory);
		}

		public static void UseRedirectMiddleware (this IAppBuilder builder, BasePathMappingRules rules)
		{
			builder.Use<RedirectMiddleware> (rules);
		}

		public static void UseUrlRewriteMiddleware (this IAppBuilder builder, BasePathMappingRules rules)
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

