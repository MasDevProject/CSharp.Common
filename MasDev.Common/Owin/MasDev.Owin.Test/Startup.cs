using Owin;

namespace MasDev.Owin.Test
{
	class Startup
	{
		public void Configuration (IAppBuilder builder)
		{
			builder.UseSimpleInjectorMiddleware ();
			builder.UseUrlRewriteMiddleware (new UrlRewriteRules ());
			builder.UseRedirectMiddleware (new RedirectRules ());
		}
	}
}

