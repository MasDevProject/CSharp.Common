using MasDev.Common.Rest.Auth;
using System;


namespace MasDev.Common.Rest
{
	public class Module :  MarshalByRefObject, IAuthorizable
	{
		readonly IRepositories _repositories;
		readonly IAuthManager _authManager = RestConfiguration.AuthOptions.Manager;



		public Module (IRepositories repositories)
		{
			_repositories = repositories;
		}



		public virtual LoginResult LogIn (ICredentials credentials, int? scope = null)
		{
			return _authManager.LogIn (credentials, scope, Repositories.CredentialsRepository);
		}



		public virtual void LogOut (ICredentials credentials)
		{
			_authManager.LogOut (credentials, Repositories.CredentialsRepository);
		}



		public int? CredentialsScope { get; set; }



		public IHttpContext HttpContext { get; set; }



		public ICredentials CurrentCredentials { get ; set; }



		public IAuthManager AuthManager { get { return _authManager; } }



		public virtual IRepositories Repositories { get { return _repositories; } }



		public virtual void Dispose ()
		{
			_repositories.Dispose ();
		}
	}
}

