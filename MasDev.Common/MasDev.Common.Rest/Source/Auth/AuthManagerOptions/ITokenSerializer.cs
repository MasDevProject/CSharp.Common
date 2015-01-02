

namespace MasDev.Rest.Auth
{
	public interface ITokenSerializer
	{
		byte[] Serialize (Token token);



		Token Deserialize (byte[] data);
	}
}

