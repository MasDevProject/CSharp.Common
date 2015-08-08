namespace MasDev.Services.Auth
{
	public interface IAccessTokenConverter
	{
		string Serialize (IAccessToken token);

		IAccessToken Deserialize (string serializedToken);
	}
}

