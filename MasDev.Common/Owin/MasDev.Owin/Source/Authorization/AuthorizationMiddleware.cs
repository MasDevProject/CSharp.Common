using Microsoft.Owin;
using System.Threading.Tasks;
using MasDev.Owin.Auth;
using System;
using System.Linq;


namespace MasDev.Owin.Middlewares
{
	public class AuthorizationMiddleware : RuledMiddleware<AuthorizationRules, AuthorizationRule, AuthorizationRulePredicate>
	{
		public const string AccessTokenOwinContextKey = "AuthorizationMiddleware.AccessToken";
		const string _accessTokenScheme = "bearer ";
		const string _authorizationHeaderName = "Authorization";

		readonly AuthorizationManager _manager;
		readonly Func<IAccessTokenStore> _storeFactory;

		public AuthorizationMiddleware (AuthorizationRules rules, AccessTokenPipeline pipeline, Func<IAccessTokenStore> storeFactory, OwinMiddleware next) : base (rules, next)
		{
			pipeline.ThrowIfNull ("pipeline");
			_manager = new AuthorizationManager (pipeline, storeFactory);
			_storeFactory = storeFactory;
			rules.Validate ();
		}


		public override async Task Invoke (IOwinContext context)
		{
			var request = context.Request;
			var method = ParseHttpMethod (request.Method);
			if (method == null) {
				await Next.Invoke (context);
				return;
			}
			var path = request.Path.ToString ();

			var matchingRule = Rules.FirstOrDefault (r => r.Method == method && r.Predicate (path));
			if (matchingRule == null) {
				await Next.Invoke (context);
				return;
			}

			int? minimumRequiredRole = null;
			if (matchingRule.RoleRestriction != null)
				minimumRequiredRole = matchingRule.RoleRestriction.AtLeastRole;

			var accessToken = GetAccessTokenFromAuthorizationHeader (context.Request);
			if (accessToken == null) {
				SendUnauthorized (context.Response);
				return;
			}

			var credentials = accessToken.Credentials;
			var lastInvalidationTimeUtc = await _storeFactory ().GetlastInvalidationUtcAsync (credentials.Id, credentials.Flag);
			if (lastInvalidationTimeUtc == null || !_manager.IsAccessTokenValid (minimumRequiredRole, lastInvalidationTimeUtc.Value, accessToken)) {
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

		static HttpMethod? ParseHttpMethod (string method)
		{
			if (string.IsNullOrWhiteSpace (method))
				return null;
			
			switch (method.ToLowerInvariant ()) {
			case HttpMethodNames.Get:
				return HttpMethod.Get;
			case HttpMethodNames.Post:
				return HttpMethod.Post;
			case HttpMethodNames.Put:
				return HttpMethod.Put;
			case HttpMethodNames.Head:
				return HttpMethod.Head;
			case HttpMethodNames.Delete:
				return HttpMethod.Delete;
			case HttpMethodNames.Trace:
				return HttpMethod.Trace;
			case HttpMethodNames.Options:
				return HttpMethod.Options;
			default:
				return null;
			}
		}
	}
}