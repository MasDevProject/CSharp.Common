namespace MasDev.Services.Auth
{
	public interface IAccessTokenCompressor
	{
		byte[] Compress (byte[] protectedAccessToken);

		byte[] Decompress (byte[] compressedAccessToken);
	}
}

