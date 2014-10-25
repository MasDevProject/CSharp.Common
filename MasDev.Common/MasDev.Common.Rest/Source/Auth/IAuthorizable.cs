using MasDev.Common.Rest.Auth;
using System;


namespace MasDev.Common.Rest
{
	public interface IAuthorizable : IDisposable
	{
		ICredentials CurrentCredentials { get; set; }



		int? CredentialsScope { get; set; }



		IHttpContext HttpContext { get; set; }



		IRepositories Repositories { get; }



		IAuthManager AuthManager { get; }
	}
}

