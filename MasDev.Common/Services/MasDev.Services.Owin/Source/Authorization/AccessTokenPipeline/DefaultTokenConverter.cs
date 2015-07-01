using Newtonsoft.Json;

namespace MasDev.Services.Auth
{
	public class DefaultAccessTokenConverter : IAccessTokenConverter
	{
		public string Serialize (IAccessToken token)
		{
			return JsonConvert.SerializeObject (token);
		}

		public IAccessToken Deserialize (string serializedToken)
		{
			return JsonConvert.DeserializeObject<AccessToken> (serializedToken);
		}
	}
}

