

namespace MasDev.Common.Rest.Auth
{
	public interface IAuthManager
	{
		AuthManagerOptions Options { get; set; }



		Token Deserialize (string headerValue);



		string Serialize (Token token);


		// returns the scope
		int Authenticate (Token token, int? roles, ICredentialsRepository repository);



		LoginResult LogIn (ICredentials credentials, int? scope, ICredentialsRepository repository);



		void LogOut (ICredentials credentials, ICredentialsRepository repository);



		ICredentials Find (int credentialsId, int flag, ICredentialsRepository repository);



		void Save (ICredentials credentials, ICredentialsRepository repository);



		string TokenType{ get; }
	}





	public class LoginResult
	{
		public string AccessToken { get; set; }



		public string TokenType { get; set; }



		public ICredentials Identity { get; set; }
	}
}

