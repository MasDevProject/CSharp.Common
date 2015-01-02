using System;
using MasDev.Rest.Auth;
using System.IO;
using System.IO.Compression;


namespace MasDev.Rest.Auth
{
	public class GZipTokenCompressor : ITokenCompressor
	{
		public string Compress (byte[] data)
		{
			using (var msi = new MemoryStream (data))
			using (var mso = new MemoryStream ())
			{
				using (var gs = new GZipStream (mso, CompressionMode.Compress))
				{
					msi.CopyTo (gs);
				}
				return Convert.ToBase64String (mso.ToArray ());
			}
		}



		public byte[] Decompress (string data)
		{
			using (var msi = new MemoryStream (Convert.FromBase64String (data)))
			using (var mso = new MemoryStream ())
			{
				using (var gs = new GZipStream (msi, CompressionMode.Decompress))
				{
					gs.CopyTo (mso);
				}
				return mso.ToArray ();
			}

		}
	}
}

