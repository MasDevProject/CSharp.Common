using System;

namespace MasDev.Owin.Auth
{
	public class AuthorizationManager
	{
		public static AuthorizationManager Current { get; private set; }

		AuthorizationManager ()
		{
			// No public constructors
		}

		readonly AccessTokenPipeline _pipeline;

		public Func<IAccessTokenStore> StoreFactory { get; private set; }

		internal AuthorizationManager (AccessTokenPipeline pipeline, Func<IAccessTokenStore> storeFactory)
		{
			pipeline.ThrowIfNull ("pipeline");
			Current = this;
			_pipeline = pipeline;
			StoreFactory = storeFactory;
		}

		public string GenerateAccessToken (int id, int roles, DateTime expirationUtc, int? scope = null)
		{
			if (expirationUtc < DateTime.UtcNow)
				throw new ArgumentException ("Expiration must be a future DateTime");
			
			var credentials = new Credentials ();
			credentials.Id = id;
			credentials.Roles = roles;

			var token = new AccessToken ();
			token.CreationUtc = DateTime.UtcNow;
			token.Credentials = credentials;
			token.ExpirationUtc = expirationUtc;
			token.Scope = scope;
			return ProcessAccessToken (token);
		}

		public string ProcessAccessToken (AccessToken token)
		{
			var serialized = _pipeline.Converter.Serialize (token);
			var cypher = _pipeline.Protector.Protect (serialized);
			var compressed = _pipeline.Compressor.Compress (cypher);
			return Convert.ToBase64String (compressed);
		}

		public AccessToken UnprocessAccessToken (string processedToken)
		{
			var bytes = Convert.FromBase64String (processedToken);
			var decompressed = _pipeline.Compressor.Decompress (bytes);
			var serialized = _pipeline.Protector.Unprotect (decompressed);
			return _pipeline.Converter.Deserialize (serialized);
		}

		public bool IsAccessTokenValid (int? minimumRequiredRoles, DateTime lastInvalidationUtc, AccessToken token)
		{
			if (token.ExpirationUtc < DateTime.UtcNow)
				return false;

			if (token.CreationUtc < lastInvalidationUtc)
				return false;

			if (minimumRequiredRoles == null)
				return true;

			return (token.Credentials.Roles & minimumRequiredRoles) >= minimumRequiredRoles;
		}
	}
}

