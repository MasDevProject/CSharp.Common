﻿using System.Threading.Tasks;
using System;
using Microsoft.Owin;
using MasDev.Services.Auth;


namespace MasDev.Services.Middlewares
{
	public class AuthorizationMiddleware : RuledMiddleware<BaseAuthorizationRules, AuthorizationRule>
	{
		public const string IdentityContextKey = "AuthorizationMiddleware.Context";
		const string _accessTokenScheme = "bearer ";
		const string _authorizationHeaderName = "Authorization";

		readonly AuthorizationManager _manager;
		readonly Func<IAccessTokenStore> _storeFactory;

		public AuthorizationMiddleware (OwinMiddleware next, BaseAuthorizationRules rules, AccessTokenPipeline pipeline, Func<IAccessTokenStore> storeFactory) : base (next, rules)
		{
			pipeline.ThrowIfNull ("pipeline");
			_manager = new AuthorizationManager (pipeline, storeFactory);
			_storeFactory = storeFactory;
			rules.Validate ();
		}


		public override async Task Invoke (IOwinContext context)
		{
			var request = context.Request;
			var method = BaseAuthorizationRules.ParseHttpMethod (request.Method);
			if (method == null) {
				await Next.Invoke (context);
				return;
			}

			var matchingRule = Rules.FindMatch (context);
			if (matchingRule == null) {
				await Next.Invoke (context);
				return;
			}
				
			var accessToken = GetAccessTokenFromAuthorizationHeader (context.Request);
			if (accessToken == null) {
				SendUnauthorized (context.Response);
				return;
			}

			var identity = accessToken.Identity;
			var lastInvalidationTimeUtc = await _storeFactory ().GetlastInvalidationUtcAsync (identity.Id, identity.Flag);
			if (lastInvalidationTimeUtc == null || !_manager.IsAccessTokenValid (matchingRule.MinimumRoles, lastInvalidationTimeUtc.Value, accessToken)) {
				SendUnauthorized (context.Response);
				return;
			}

			var language = "en-us"; // TODO
			context.Set (IdentityContextKey, new IdentityContext (accessToken.Identity, language, accessToken.Scope));
			await Next.Invoke (context);
		}

		static void SendUnauthorized (IOwinResponse response)
		{
			response.Body = null;
			response.ContentLength = null;
			response.StatusCode = 401;
		}

		AccessToken GetAccessTokenFromAuthorizationHeader (IOwinRequest request)
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
				return _manager.UnprocessAccessToken (headerAccessToken);
			} catch (Exception) {
				return null;
			}
		}
	}
}