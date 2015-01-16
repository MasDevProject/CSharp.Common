

namespace MasDev.Rest.Auth
{
	public interface ITokenCompressor
	{
		string Compress (byte[] data);



		byte[] Decompress (string data);
	}
}

