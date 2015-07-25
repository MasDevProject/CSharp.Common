using System;
using Owin;
using MasDev.Patterns.Injection.SimpleInjector;

namespace MasDev.Services.Middlewares
{
	public static class Extensions
	{
		public static void UseAuthorizationMiddleware (this IAppBuilder app)
		{
			app.Use<AuthorizationMiddleware> ();
		}

		public static void UseRedirectMiddleware (this IAppBuilder builder, BasePathMappingRules rules)
		{
			builder.Use<RedirectMiddleware> (rules);
		}

		public static void UseRedirectMiddleware (this IAppBuilder builder, string configFilePath)
		{
			builder.Use<RedirectMiddleware> (configFilePath);
		}


		public static void UseUrlRewriteMiddleware (this IAppBuilder builder, BasePathMappingRules rules)
		{
			builder.Use<UrlRewriteMiddleware> (rules);
		}


		public static void UseUrlRewriteMiddleware (this IAppBuilder builder, string configFilePath)
		{
			builder.Use<UrlRewriteMiddleware> (configFilePath);
		}

		public static void UseSimpleInjectorMiddleware (this IAppBuilder builder, SimpleInjectorContainer container)
		{
			builder.Use<SimpleInjectorMiddleware> (container);
		}

		public static void UseUnitOfWorkHandlerMiddleware (this IAppBuilder builder, params int[] committableStatusCodes)
		{
			builder.Use<UnitOfWorkHandlerMiddleware> (committableStatusCodes);
		}


		public static void UseDiagnosticMiddleware (this IAppBuilder builder)
		{
			builder.Use<DiagnosticMiddleware> ();
		}

		public static void UseCallingContextMiddleware (this IAppBuilder builder, string fallbackLanguage)
		{
			builder.Use<CallingContextMiddleware> (fallbackLanguage);
		}

		public static void ThrowIfNull (this object obj, string name)
		{
			if (obj == null)
				throw new ArgumentNullException (name);
		}
	}
}

