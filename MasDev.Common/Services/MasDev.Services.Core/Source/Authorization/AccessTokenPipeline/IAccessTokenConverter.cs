namespace MasDev.Services.Auth
{
	public interface IAccessTokenConverter
	{
		string Serialize (AccessToken token);

		AccessToken Deserialize (string serializedToken);
	}
}

