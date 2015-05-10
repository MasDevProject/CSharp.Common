using Owin;
using MasDev.Owin.Auth;

namespace MasDev.Owin.Test
{
	class Startup
	{
		public void Configuration (IAppBuilder builder)
		{
			builder.UseSimpleInjectorMiddleware ();
			builder.UseUrlRewriteMiddleware (new UrlRewriteRules ());
			builder.UseRedirectMiddleware (new RedirectRules ());
			builder.UseAuthorizationMiddleware (new AuthorizationRules (), new DefaultAccessTokenPipeline ("pwd"), () => new DummyAccessTokenStore ());
		}
	}
}

