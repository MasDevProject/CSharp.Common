using System;
using MasDev.Services.Modeling;
using MasDev.Patterns.Injection;
using MasDev.Common;
using System.Threading.Tasks;

namespace MasDev.Services.Auth
{
	public interface IAuthorizationManager
	{
		string GenerateAccessToken (int id, int roles, DateTime expirationUtc, int? scope = null);

		string ProcessAccessToken (IAccessToken token);

		IAccessToken UnprocessAccessToken (string processedToken);

		Task AuthorizeAsync (int? minimumRequiredRoles = null);
	}

	public class AuthorizationManager : IAuthorizationManager
	{
		public static AuthorizationManager Current { get; private set; }

		readonly AccessTokenPipeline _pipeline;
		readonly Func<IAccessTokenStore> _storeFactory;

		public AuthorizationManager (AccessTokenPipeline pipeline, Func<IAccessTokenStore> storeFactory)
		{
			if (pipeline == null)
				throw new ArgumentNullException ("pipeline");
			Current = this;
			_pipeline = pipeline;
			_storeFactory = storeFactory;
		}

		public string GenerateAccessToken (int id, int roles, DateTime expirationUtc, int? scope = null)
		{
			if (expirationUtc < DateTime.UtcNow)
				throw new ArgumentException ("Expiration must be a future DateTime");
			
			var identity = new Identity ();
			identity.Id = id;
			identity.Roles = roles;

			var token = new AccessToken ();
			token.CreationUtc = DateTime.UtcNow;
			token.Identity = identity;
			token.ExpirationUtc = expirationUtc;
			token.Scope = scope;
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
			var identityContext = Injector.Resolve<IIdentityContext> ();
			var accessToken = Injector.Resolve<IAccessToken> ();

			if (identityContext == null)
				throw new UnauthorizedException ();

			var identity = identityContext.Identity;
			if (identity == null)
				throw new UnauthorizedException ();
			
			var lastInvalidationTimeUtc = await _storeFactory ().GetlastInvalidationUtcAsync (identity.Id, identity.Flag);
			if (lastInvalidationTimeUtc == null || !IsAccessTokenValid (minimumRequiredRoles, lastInvalidationTimeUtc.Value, accessToken))
				throw new UnauthorizedException ();
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

