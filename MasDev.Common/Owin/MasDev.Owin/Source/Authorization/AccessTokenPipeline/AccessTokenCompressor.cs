
namespace MasDev.Owin.Auth
{
	public interface IAccessTokenCompressor
	{
		byte[] Compress (byte[] protectedAccessToken);

		byte[] Decompress (byte[] compressedAccessToken);
	}

	public class DefaultAccessTokenCompressor : IAccessTokenCompressor
	{
		public byte[] Compress (byte[] protectedAccessToken)
		{
			throw new System.NotImplementedException ();
		}

		public byte[] Decompress (byte[] compressedAccessToken)
		{
			throw new System.NotImplementedException ();
		}
	}
}

