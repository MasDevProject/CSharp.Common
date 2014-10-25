using MasDev.Common.Rest;
using System;
using Newtonsoft.Json;
using MasDev.Common.Http;
using MasDev.Common.Security;


namespace MasDev.Common.Rest
{
	public class AuthToken<TCredentials> : IAuthToken<TCredentials> where TCredentials : ICredentials
	{
		const string _cypherPassword = "zxqw23TYU7?!";



		public void Read (string authorizationHeaderValue)
		{
			var s = authorizationHeaderValue.Replace (AuthorizationHeader.BearerScheme + "=", string.Empty);
			var json = Cypher.Decrypt (s, _cypherPassword);
			var token = JsonConvert.DeserializeObject<AuthToken<TCredentials>> (json);
			SessionId = token.SessionId;
			Credentials = token.Credentials;
			Expires = token.Expires;
			Creation = token.Creation;
			Scope = token.Scope;
		}



		public string SessionId { get; set; }



		public TCredentials Credentials { get; set; }



		public DateTime Expires{ get; set; }



		public DateTime Creation{ get; set; }



		public byte Scope { get; set; }



		public ISymmetricCrypto Cypher { get; set; }





		public string ToHeaderValue ()
		{
			var cypher = Cypher;
			Cypher = null;
			var json = JsonConvert.SerializeObject (this, new JsonSerializerSettings {
				NullValueHandling = NullValueHandling.Ignore
			});
			Cypher = cypher;
			return "auth_token=" + Cypher.Encrypt (json, _cypherPassword);
		}
	}
}

