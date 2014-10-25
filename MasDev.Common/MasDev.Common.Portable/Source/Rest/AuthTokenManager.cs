using MasDev.Common.Rest;
using MasDev.Common.Security;
using System;
using MasDev.Common.Collections;


namespace MasDev.Common.Rest
{
	public class AuthTokenManager<TCredentials> : IAuthTokenManager<TCredentials> where TCredentials : class, ICredentials
	{
		readonly ConcurrentHashSet<string> _activeSessions = new ConcurrentHashSet<string> ();



		public IAuthToken<TCredentials> ProvideToken (TCredentials credentials, IExpirationStrategy expirationStrategy, byte scope = 0)
		{
			var token = new AuthToken<TCredentials> ();
			token.Cypher = Cypher;

			var now = DateTime.UtcNow;
			var expiration = expirationStrategy.GetExpirationDate (credentials, now);

			token.Creation = now;
			token.Credentials = credentials;
			token.Expires = expiration;
			token.Scope = scope;

			_activeSessions.Add (credentials.Session);
			return token;
		}



		public IAuthToken<TCredentials> ReadToken (string authorizationHeaderValue)
		{
			if (authorizationHeaderValue == null)
				return null;
			var token = new AuthToken<TCredentials> ();
			token.Cypher = Cypher;
			token.Read (authorizationHeaderValue);
			return token;
		}



		public TokenValidation Validate (IAuthToken<TCredentials> token, ICredentialsRepository<TCredentials> credentialsRepository)
		{
			if (token == null)
				return new TokenValidation (false, TokenValidationFailure.Missing);

			var now = DateTime.UtcNow;

			if (now > token.Expires)
				return new TokenValidation (false, TokenValidationFailure.Expired);

			var credentials = token.Credentials;
			var session = credentials.Session;

			lock (_activeSessions)
			{
				if (!_activeSessions.Contains (session))
				{
					credentials = credentialsRepository.FromSession (session);
					if (credentials == null)
						return new TokenValidation (false, TokenValidationFailure.SessionInvalidated);

					_activeSessions.Add (session);
				}
			}

			return new TokenValidation (true, TokenValidationFailure.None);
		}



		public void Invalidate (string session)
		{
			lock (_activeSessions)
			{
				_activeSessions.Remove (session);
			}
		}



		public ISymmetricCrypto Cypher { get; set; }
	}
}

