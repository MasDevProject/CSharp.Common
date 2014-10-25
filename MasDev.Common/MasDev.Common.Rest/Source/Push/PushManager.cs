using System.Threading.Tasks;
using MasDev.Common.Rest.Auth;
using MasDev.Common.Rest.Push;
using MasDev.Common.Utils;
using System;


namespace MasDev.Common.Rest
{
	public abstract class PushManager : MarshalByRefObject, IAuthorizable
	{
		readonly IRepositories _repositories;
		readonly IAuthManager _authManager = RestConfiguration.AuthOptions.Manager;
		readonly IConnectionManager _connectionManager;



		protected PushManager (IRepositories repositories)
		{
			_repositories = repositories;
			PushContext.RegisterConnectionManager (GetType (), CreateConnectionManager);
			_connectionManager = CreateConnectionManager (_repositories);
		}



		public int? CredentialsScope { get; set; }



		IHttpContext IAuthorizable.HttpContext
		{
			get { return HttpContext; }
			set { HttpContext = Assert.As<IPushHttpContext> (value); }
		}



		public IPushHttpContext HttpContext	{ get; set; }



		public ICredentials CurrentCredentials { get ; set; }



		public IAuthManager AuthManager { get { return _authManager; } }



		public virtual IRepositories Repositories { get { return _repositories; } }



		protected IConnectionManager Connections { get { return _connectionManager; } }



		protected abstract IConnectionManager CreateConnectionManager (IRepositories repositories);



		public virtual void Dispose ()
		{
			_repositories.Dispose ();
		}



		public abstract Task OnConnected ();



		public abstract Task OnDisconnected (bool stopCalled);



		public abstract Task OnReconnected ();
	}
}

