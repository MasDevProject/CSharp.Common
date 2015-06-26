using System;
using System.Collections.Concurrent;
using MasDev.Utils;


namespace MasDev.Rest.Auth
{
	public class BaseAuthManager : IAuthManager
	{
		// Tuple<id, flag>
		readonly ConcurrentDictionary<Tuple<int, int>, ICredentials> _issued = new ConcurrentDictionary<Tuple<int, int>, ICredentials> ();

		public Token Deserialize (string headerValue)
		{
			try {
				var decompressed = TokenCompressor.Decompress (headerValue);
				var unprotected = TokenProtector.Unprotect (decompressed);
				return TokenSerializer.Deserialize (unprotected);
			} catch {
				return null;
			}
		}

		public string Serialize (Token token)
		{
			var serialized = TokenSerializer.Serialize (token);
			var protect = TokenProtector.Protect (serialized);
			var ret = TokenCompressor.Compress (protect);
			return ret;
		}

		public int Authenticate (Token token, int? roles, ICredentialsRepository repository)
		{
			if (token == null)
				throw new UnauthorizedException ("Missing authorization token");

			if (token.ExpiresUtc < DateTime.UtcNow)
				throw new UnauthorizedException ("Token expired");
				
			var tokenCredentials = token.Credentials;
			var issuedCredentials = Find (tokenCredentials.Id, tokenCredentials.Flag, repository);

			if (issuedCredentials == null)
				throw new UnauthorizedException ("Credentials have been revoked");

			if (issuedCredentials.LastIssuedUTC > tokenCredentials.LastIssuedUTC)
				throw new UnauthorizedException ("Authorization token has been revoked");
				
			if (!issuedCredentials.IsEnabled)
				throw new UnauthorizedException ("Credentials no longer enabled");

			if (roles == null)
				return token.Scope;
				
			var rolesIntersection = roles & issuedCredentials.Roles;
			if (rolesIntersection != roles)
				throw new UnauthorizedException ("Invald authorization level");

			return token.Scope;
		}

		public LoginResult LogIn (ICredentials credentials, int? scope, ICredentialsRepository repository)
		{
			using (new LockByIdMutex (credentials.Id)) {
				Save (credentials, repository);

				var token = new Token {
					Credentials = TokenCredentials.Import (credentials),
					ExpiresUtc = credentials.LastIssuedUTC + Expiration.GetExpiration (credentials),
					Scope = scope ?? 0
				};

				return new LoginResult {
					AccessToken = Serialize (token),
					TokenType = TokenType,
					Identity = credentials
				};
			}
		}

		public void LogOut (ICredentials credentials, ICredentialsRepository repository)
		{
			using (new LockByIdMutex (credentials.Id)) {
				credentials.LastIssuedUTC = DateTime.UtcNow;
				var credentialsKey = Tuple.Create (credentials.Id, credentials.Flag);
				_issued.AddOrUpdate (
					credentialsKey,
					credentials,
					(key, old) => credentials
				);
				repository.Update (credentials);
			}
		}

		public ICredentials Find (int credentialsId, int flag, ICredentialsRepository repository)
		{
			using (new LockByIdMutex (credentialsId)) {
				var credentialsKey = Tuple.Create (credentialsId, flag);
				if (_issued.ContainsKey (credentialsKey))
					return _issued [credentialsKey];

				var credentials = repository.Read (credentialsId, flag);
				if (credentials != null)
					_issued.AddOrUpdate (
						credentialsKey,
						credentials,
						(key, old) => credentials
					);
				return credentials;
			}
		}

		public void Save (ICredentials credentials, ICredentialsRepository repository)
		{
			var credentialsKey = Tuple.Create (credentials.Id, credentials.Flag);
			_issued.AddOrUpdate (
				credentialsKey,
				credentials,
				(key, old) => credentials
			);
			repository.Update (credentials);
		}

		public void ClearCache ()
		{
			_issued.Clear ();
		}

		public AuthManagerOptions Options { get; set; }

		const string _tokenType = "bearer";

		public string TokenType { get { return _tokenType; } }

		ITokenCompressor TokenCompressor { get { return Options.TokenCompressor; } }

		ITokenProtector TokenProtector { get { return Options.TokenProtector; } }

		ITokenSerializer TokenSerializer { get { return Options.TokenSerializer; } }

		IExpiration Expiration { get { return Options.Expiration; } }
	}

	class TokenCredentials : ICredentials
	{
		public int Roles { get; set; }

		public DateTime LastIssuedUTC { get; set; }

		public bool IsEnabled { get; set; }

		public int Id { get; set; }

		public int Flag { get; set; }

		public static TokenCredentials Import (ICredentials credentials)
		{
			return new TokenCredentials {
				Roles = credentials.Roles,
				LastIssuedUTC = credentials.LastIssuedUTC,
				IsEnabled = credentials.IsEnabled,
				Id = credentials.Id,
				Flag = credentials.Flag,
			};
		}
	}
}

