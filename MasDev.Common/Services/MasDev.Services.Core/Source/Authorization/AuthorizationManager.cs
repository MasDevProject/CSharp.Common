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
		// TODO caching
		const string _tokenType = "bearer";
		readonly AccessTokenPipeline _pipeline;
		readonly Func<ICredentialsRepository> _credentialsRepositoryFactory;

		public AuthorizationManager (AccessTokenPipeline pipeline, Func<ICredentialsRepository> storeFactory)
		{
			if (pipeline == null)
				throw new ArgumentNullException ("pipeline");
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

		public async Task AuthorizeAsync (int? minimumRequiredRoles = null)
		{
			var callingContext = Injector.Resolve<ICallingContext> ();
			var accessToken = Injector.Resolve<IAccessToken> ();

			if (callingContext == null)
				throw new UnauthorizedException ();

			var identity = callingContext.Identity;
			if (identity == null)
				throw new UnauthorizedException ();
			
			var lastInvalidationTimeUtc = await _credentialsRepositoryFactory ().GetlastInvalidationUtcAsync (identity.Id, identity.Flag);
			if (lastInvalidationTimeUtc == null || !IsAccessTokenValid (minimumRequiredRoles, lastInvalidationTimeUtc.Value, accessToken))
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
			// TODO
		}

		static bool IsAccessTokenValid (int? minimumRequiredRoles, DateTime lastInvalidationUtc, IAccessToken token)
		{
			if (token.ExpirationUtc < DateTime.UtcNow)
				return false;

			if (token.CreationUtc < lastInvalidationUtc)
				return false;

			if (minimumRequiredRoles == null)
				return true;

			return (token.Identity.Roles & minimumRequiredRoles) >= minimumRequiredRoles;
		}
	}
}

