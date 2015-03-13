using System;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;


namespace MasDev.Rest.Auth
{
	public class JSonTokenSerializer : ITokenSerializer
	{
		public byte[] Serialize (Token token)
		{
			var serialized = JsonConvert.SerializeObject (token);
			return Encoding.UTF8.GetBytes (serialized);
		}



		public Token Deserialize (byte[] data)
		{
			var serialized = Encoding.UTF8.GetString (data);
			var json = JObject.Parse (serialized);

			var token = new Token ();
			token.ExpiresUtc = json ["ExpiresUtc"].Value<DateTime> ();
			token.Scope = json ["Scope"].Value<int> ();

			var credentialsJson = json ["Credentials"].ToString ();
			var credentials = JsonConvert.DeserializeObject<TokenCredentials> (credentialsJson);
			token.Credentials = credentials;

			var extraJson = json ["Extra"];
			token.Extra = extraJson;

			return token;
		}
	}
}

