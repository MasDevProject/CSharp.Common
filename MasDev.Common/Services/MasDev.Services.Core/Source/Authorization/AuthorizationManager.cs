using System;
using MasDev.Services.Modeling;
using MasDev.Patterns.Injection;
using MasDev.Common;
using System.Threading.Tasks;

namespace MasDev.Services.Auth
{
	public interface IAuthorizationManager
	{
		string TokenScheme { get; }

		string GenerateAccessToken (Identity identity, DateTime issuedUtc, TimeSpan duration, int? extra = null, int? scope = null);

		string ProcessAccessToken (IAccessToken token);

		IAccessToken UnprocessAccessToken (string processedToken);

		Task AuthorizeAsync (int? minimumRequiredRoles = null);

		Task<DateTime> RenewIssueTimeAsync (Identity identity);

		void InvalidateCache ();
	}

	public class AuthorizationManager : IAuthorizationManager
	{
		const string _tokenType = "bearer";
		readonly AccessTokenPipeline _pipeline;
		readonly Func<IIdentityRepository> _credentialsRepositoryFactory;

		public AuthorizationManager (AccessTokenPipeline pipeline, Func<IIdentityRepository> storeFactory)
		{
			if (pipeline == null)
				throw new ArgumentNullException (nameof (pipeline));

			_pipeline = pipeline;
			_credentialsRepositoryFactory = storeFactory;
		}

		public string TokenScheme { get { return _tokenType; } }

		public string GenerateAccessToken (Identity identity, DateTime issuedUtc, TimeSpan duration, int? extra = null, int? scope = null)
		{
			var token = new AccessToken ();
			token.CreationUtc = issuedUtc;
			token.Identity = identity;
			token.ExpirationUtc = issuedUtc + duration;
			token.Scope = scope;
			token.Extra = extra;
			return ProcessAccessToken (token);
		}

		public string ProcessAccessToken (IAccessToken token)
		{
			var serialized = _pipeline.Converter.Serialize (token);
			var cypher = _pipeline.Protector.Protect (serialized);
			var compressed = _pipeline.Compressor.Compress (cypher);
			return Convert.ToBase64String (compressed);
		}

		public IAccessToken UnprocessAccessToken (string processedToken)
		{
			var bytes = Convert.FromBase64String (processedToken);
			var decompressed = _pipeline.Compressor.Decompress (bytes);
			var serialized = _pipeline.Protector.Unprotect (decompressed);
			return _pipeline.Converter.Deserialize (serialized);
		}

		public async Task AuthorizeAsync (int? requiredRole = null)
		{
			var callingContext = Injector.Resolve<ICallingContext> ();
			var accessToken = Injector.Resolve<IAccessToken> ();

			if (callingContext == null || accessToken == null)
				throw new UnauthorizedException ();

			var identity = callingContext.Identity;
			if (identity == null || accessToken.Identity == null)
				throw new UnauthorizedException ();

			if (!(identity.Id == accessToken.Identity.Id && identity.Flag == accessToken.Identity.Flag))
				throw new UnauthorizedException ();

			var lastInvalidationTimeUtc = await _credentialsRepositoryFactory ().GetlastInvalidationUtcAsync (identity.Id, identity.Flag);

			if (lastInvalidationTimeUtc == null || !IsAccessTokenValid (requiredRole, lastInvalidationTimeUtc.Value, accessToken))
				throw new UnauthorizedException ();
		}

		public async Task<DateTime> RenewIssueTimeAsync (Identity identity)
		{
			var now = DateTime.UtcNow;
			await _credentialsRepositoryFactory ().SetInvalidationTime (identity.Id, identity.Flag, now);
			return now;
		}

		public void InvalidateCache ()
		{
			var store = _credentialsRepositoryFactory () as ICachedIdentityRepository;
			if (store != null)
				store.ClearCache ();
		}

		static bool IsAccessTokenValid (int? requiredRole, DateTime lastInvalidationUtc, IAccessToken token)
		{
			if (token.ExpirationUtc < DateTime.UtcNow)
				return false;

			if (token.CreationUtc < lastInvalidationUtc)
				return false;

			if (requiredRole == null)
				return true;

			var isValid = (token.Identity.Roles & requiredRole) == requiredRole;
			return isValid;
		}
	}

}

