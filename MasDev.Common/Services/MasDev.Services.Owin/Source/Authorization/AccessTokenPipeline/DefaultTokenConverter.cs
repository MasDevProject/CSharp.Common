using Newtonsoft.Json;

namespace MasDev.Services.Auth
{
	public class DefaultAccessTokenConverter : IAccessTokenConverter
	{
		public string Serialize (AccessToken token)
		{
			return JsonConvert.SerializeObject (token);
		}

		public AccessToken Deserialize (string serializedToken)
		{
			return JsonConvert.DeserializeObject<AccessToken> (serializedToken);
		}
	}
}

