using MasDev.Rest.Auth;
using System;


namespace MasDev.Rest
{
	public interface IRestModule : IDisposable
	{
		ICredentials CurrentCredentials { get; set; }



		int? CredentialsScope { get; set; }



		IHttpContext HttpContext { get; set; }



		IRepositories Repositories { get; }



		IAuthManager AuthManager { get; }
	}
}

