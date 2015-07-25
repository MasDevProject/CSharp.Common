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
		const string _authorizationHeaderName = "Authorization";

		public AuthorizationMiddleware (OwinMiddleware next) : base (next)
		{
		}

		public override async Task Invoke (IOwinContext context)
		{
			var callingContext = Injector.Resolve<ICallingContext> ();
		
			var manager = Injector.Resolve<IAuthorizationManager> ();
			var accessToken = GetAccessTokenFromAuthorizationHeader (context.Request, manager.TokenScheme);
			if (accessToken == null)
				return;

			var injectedToken = Injector.Resolve<IAccessToken> ();
			Mapper.DynamicMap<IAccessToken, IAccessToken> (accessToken, injectedToken);

			// TODO muoverlo in un nuovo middleware
			callingContext.Language = "en-us"; // TODO
			callingContext.Scope = accessToken.Scope;
			callingContext.Identity = accessToken.Identity;
			callingContext.RequestPath = context.Request.Uri.AbsoluteUri;
			callingContext.RequestIp = context.Request.RemoteIpAddress;
			callingContext.RequestHost = context.Request.Host.Value;

			try {
				await Next.Invoke (context);
			} catch (Exception e) {
				if (e is UnauthorizedException)
					SendUnauthorized (context.Response);
				else
					throw;
			}
		}

		static IAccessToken GetAccessTokenFromAuthorizationHeader (IOwinRequest request, string tokenScheme)
		{
			var headers = request.Headers;
			if (!headers.ContainsKey (_authorizationHeaderName))
				return null;

			var schemedAccessToken = headers.Get (_authorizationHeaderName);
			if (string.IsNullOrWhiteSpace (schemedAccessToken))
				return null;

			var schemeIndex = schemedAccessToken.IndexOf (tokenScheme, StringComparison.OrdinalIgnoreCase);
			if (schemeIndex < 0)
				return null;

			var headerAccessToken = schemedAccessToken.Substring (tokenScheme.Length - 1);

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
			container.AddDependency<ICallingContext, CallingContext> (perRequestLifeStyle);
			container.AddDependency<IAccessToken, AccessToken> (perRequestLifeStyle);
		}
	}
}