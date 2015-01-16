

namespace MasDev.Rest.Auth
{
	public interface ITokenProtector
	{
		byte[] Protect (byte[] data);



		byte[] Unprotect (byte[] data);
	}
}

