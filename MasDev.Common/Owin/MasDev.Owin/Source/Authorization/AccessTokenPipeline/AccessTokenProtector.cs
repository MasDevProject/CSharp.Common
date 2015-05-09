
namespace MasDev.Owin.Auth
{
	public interface IAccessTokenProtector
	{
		byte[] Protect (string serializedAccessToken);

		string Unprotect (byte[] protectedAccessToken);
	}

	public class DefaultAccessTokenProtector : IAccessTokenProtector
	{
		readonly string _password;

		public DefaultAccessTokenProtector (string password)
		{
			_password = password;
		}


		public byte[] Protect (string serializedAccessToken)
		{
			throw new System.NotImplementedException ();
		}

		public string Unprotect (byte[] protectedAccessToken)
		{
			throw new System.NotImplementedException ();
		}
	}
}

