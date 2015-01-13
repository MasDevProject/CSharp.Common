using System.Threading.Tasks;
using MasDev.Rest.Auth;
using MasDev.Rest.Push;
using MasDev.Utils;
using System;
using System.Collections.Generic;
using MasDev.Data;


namespace MasDev.Rest
{
	public abstract class PushManager : MarshalByRefObject, IRestModule
	{
		readonly IRepositories _repositories;
		readonly IAuthManager _authManager = RestConfiguration.AuthOptions.Manager;
		readonly Dictionary<Type, dynamic> _connectionManagers;

		protected PushManager (IRepositories repositories)
		{
			_repositories = repositories;
			_connectionManagers = new Dictionary<Type, dynamic> ();
		}

		public int? CredentialsScope { get; set; }

		IHttpContext IRestModule.HttpContext {
			get { return HttpContext; }
			set { HttpContext = Assert.As<IPushHttpContext> (value); }
		}

		public IPushHttpContext HttpContext	{ get; set; }

		public ICredentials CurrentCredentials { get ; set; }

		public IAuthManager AuthManager { get { return _authManager; } }

		public virtual IRepositories Repositories { get { return _repositories; } }

		protected IConnectionManager<TModel> Connections<TModel> () where TModel : class, IModel, new()
		{
			var type = typeof(TModel);
			if (!_connectionManagers.ContainsKey (type))
				_connectionManagers.Add (type, PushContext.ConnectionManagerInternal<TModel> (_repositories));

			return _connectionManagers [type];
		}

		public virtual void Dispose ()
		{
			_repositories.Dispose ();
		}

		public abstract Task OnConnected ();

		public abstract Task OnDisconnected (bool stopCalled);

		public abstract Task OnReconnected ();
	}
}

