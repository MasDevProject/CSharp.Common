using System.Threading.Tasks;
using System;
using Microsoft.Owin;
using MasDev.Common.Owin.Rules;
using MasDev.Common.Owin.Auth;


namespace MasDev.Common.Owin.Middlewares
{
	public class AuthorizationMiddleware : RuledMiddleware<AuthorizationRules, AuthorizationRule>
	{
		public const string AccessTokenOwinContextKey = "AuthorizationMiddleware.AccessToken";
		const string _accessTokenScheme = "bearer ";
		const string _authorizationHeaderName = "Authorization";

		readonly AuthorizationManager _manager;
		readonly Func<IAccessTokenStore> _storeFactory;

		public AuthorizationMiddleware (OwinMiddleware next, AuthorizationRules rules, AccessTokenPipeline pipeline, Func<IAccessTokenStore> storeFactory) : base (next, rules)
		{
			pipeline.ThrowIfNull ("pipeline");
			_manager = new AuthorizationManager (pipeline, storeFactory);
			_storeFactory = storeFactory;
			rules.Validate ();
		}


		public override async Task Invoke (IOwinContext context)
		{
			var request = context.Request;
			var method = AuthorizationRules.ParseHttpMethod (request.Method);
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

			var credentials = accessToken.Credentials;
			var lastInvalidationTimeUtc = await _storeFactory ().GetlastInvalidationUtcAsync (credentials.Id, credentials.Flag);
			if (lastInvalidationTimeUtc == null || !_manager.IsAccessTokenValid (matchingRule.MinimumRoles, lastInvalidationTimeUtc.Value, accessToken)) {
				SendUnauthorized (context.Response);
				return;
			}

			context.Set (AccessTokenOwinContextKey, accessToken);
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