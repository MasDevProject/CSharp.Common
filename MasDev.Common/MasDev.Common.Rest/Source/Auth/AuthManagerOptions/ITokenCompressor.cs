

namespace MasDev.Common.Rest.Auth
{
	public interface ITokenCompressor
	{
		string Compress (byte[] data);



		byte[] Decompress (string data);
	}
}

