using System;
using MasDev.Common.Rest.Auth;
using System.Collections.Concurrent;
using MasDev.Common.Mono;


namespace MasDev.Common.Rest.Auth
{
	public class BaseAuthManager : IAuthManager
	{
		readonly ConcurrentDictionary<int, ICredentials> _issued = new ConcurrentDictionary<int, ICredentials> ();



		public Token Deserialize (string headerValue)
		{
			try {
				var decompressed = TokenCompressor.Decompress (headerValue);
				var unprotected = TokenProtector.Unprotect (decompressed);
				return TokenSerializer.Deserialize (unprotected);
			} catch (Exception e) {
				Console.WriteLine (e);
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
			var issuedCredentials = Find (tokenCredentials.Id, repository);

			if (issuedCredentials == null)
				throw new UnauthorizedException ("Credentials have been revoked");

			if (issuedCredentials.LastIssuedUTC > tokenCredentials.LastIssuedUTC)
				throw new UnauthorizedException ("Authorization token has been revoked");
				
			if (!issuedCredentials.IsEnabled)
				throw new UnauthorizedException ("Credentials no longer enabled");

			if (roles == null)
				return token.Scope;
				
			if ((roles & issuedCredentials.Roles) == 0)
				throw new UnauthorizedException ("Invald authorization level");

			return token.Scope;
		}



		public LoginResult LogIn (ICredentials credentials, int? scope, ICredentialsRepository repository)
		{
			using (new MutexEx (credentials.Id)) {
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
			using (new MutexEx (credentials.Id)) {
				credentials.LastIssuedUTC = DateTime.UtcNow;
				_issued.AddOrUpdate (
					credentials.Id,
					id => credentials,
					(id, old) => credentials
				);
				repository.Update (credentials);
			}
		}




		public ICredentials Find (int credentialsId, ICredentialsRepository repository)
		{
			using (new MutexEx (credentialsId)) {
				if (_issued.ContainsKey (credentialsId))
					return _issued [credentialsId];

				var credentials = repository.Read (credentialsId);
				if (credentials != null)
					_issued.AddOrUpdate (
						credentialsId,
						id => credentials,
						(id, old) => credentials
					);
				return credentials;
			}
		}



		public void Save (ICredentials credentials, ICredentialsRepository repository)
		{
			credentials.LastIssuedUTC = DateTime.UtcNow;
			_issued.AddOrUpdate (
				credentials.Id,
				id => credentials,
				(id, old) => credentials
			);
			repository.Update (credentials);
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



		public static TokenCredentials Import (ICredentials credentials)
		{
			return new TokenCredentials {
				Roles = credentials.Roles,
				LastIssuedUTC = credentials.LastIssuedUTC,
				IsEnabled = credentials.IsEnabled,
				Id = credentials.Id
			};
		}
	}
}

