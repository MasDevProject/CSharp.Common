using System.Threading.Tasks;
using System;
using Microsoft.Owin;
using MasDev.Services.Auth;
using MasDev.Patterns.Injection;
using MasDev.Common;
using AutoMapper;

namespace MasDev.Services.Middlewares
{
	public class AuthorizationMiddleware : OwinMiddleware
	{
		const string _accessTokenScheme = "bearer ";
		const string _authorizationHeaderName = "Authorization";

		public AuthorizationMiddleware (OwinMiddleware next) : base (next)
		{
		}

		public override async Task Invoke (IOwinContext context)
		{
			var identityContext = Injector.Resolve<IIdentityContext> ();
		
			var accessToken = GetAccessTokenFromAuthorizationHeader (context.Request);
			if (accessToken == null)
				return;

			var injectedToken = Injector.Resolve<IAccessToken> ();
			Mapper.DynamicMap<IAccessToken, IAccessToken> (accessToken, injectedToken);

			identityContext.Language = "en-us"; // TODO
			identityContext.Scope = accessToken.Scope;
			identityContext.Identity = accessToken.Identity;

			try {
				await Next.Invoke (context);
			} catch (Exception e) {
				if (e is UnauthorizedException)
					SendUnauthorized (context.Response);
				else
					throw;
			}
		}

		static IAccessToken GetAccessTokenFromAuthorizationHeader (IOwinRequest request)
		{
			var headers = request.Headers;
			if (!headers.ContainsKey (_authorizationHeaderName))
				return null;

			var schemedAccessToken = headers.Get (_authorizationHeaderName);
			if (string.IsNullOrWhiteSpace (schemedAccessToken))
				return null;

			var schemeIndex = schemedAccessToken.IndexOf (_accessTokenScheme, StringComparison.OrdinalIgnoreCase);
			if (schemeIndex < 0)
				return null;

			var headerAccessToken = schemedAccessToken.Substring (_accessTokenScheme.Length - 1);

			try {
				var manager = Injector.Resolve<IAuthorizationManager> ();
				return manager.UnprocessAccessToken (headerAccessToken);
			} catch (Exception) {
				return null;
			}
		}

		static void SendUnauthorized (IOwinResponse response)
		{
			response.Body = null;
			response.ContentLength = null;
			response.StatusCode = 401;
		}

		public static void RegisterDependencies (IDependencyContainer container, object perRequestLifeStyle)
		{
			container.AddDependency<IIdentityContext, IdentityContext> (perRequestLifeStyle);
			container.AddDependency<IAccessToken, AccessToken> (perRequestLifeStyle);
		}
	}
}