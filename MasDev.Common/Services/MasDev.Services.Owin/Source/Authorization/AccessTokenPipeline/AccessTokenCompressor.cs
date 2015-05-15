using System.IO;
using System.IO.Compression;


namespace MasDev.Services.Auth
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
			using (var msi = new MemoryStream (protectedAccessToken))
			using (var mso = new MemoryStream ()) {
				using (var gs = new GZipStream (mso, CompressionMode.Compress)) {
					msi.CopyTo (gs);
				}
				return mso.ToArray ();
			}
		}

		public byte[] Decompress (byte[] compressedAccessToken)
		{
			using (var msi = new MemoryStream (compressedAccessToken))
			using (var mso = new MemoryStream ()) {
				using (var gs = new GZipStream (msi, CompressionMode.Decompress)) {
					gs.CopyTo (mso);
				}
				return mso.ToArray ();
			}
		}
	}
}

