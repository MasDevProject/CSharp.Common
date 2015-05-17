namespace MasDev.Services.Auth
{
	public interface IAccessTokenProtector
	{
		byte[] Protect (string serializedAccessToken);

		string Unprotect (byte[] protectedAccessToken);
	}
}