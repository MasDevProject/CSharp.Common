using Newtonsoft.Json;

namespace MasDev.Common.Owin.Auth
{
	public interface IAccessTokenConverter
	{
		string Serialize (AccessToken token);

		AccessToken Deserialize (string serializedToken);
	}

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

