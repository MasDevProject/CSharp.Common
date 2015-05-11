using SimpleInjector;
using System;
using SimpleInjector.Advanced;
using System.Runtime.Remoting.Messaging;

namespace MasDev.Common.Owin
{
	public class PerRequestLifestyle : ScopedLifestyle
	{
		#region Instance

		static Lazy<PerRequestLifestyle> _instance = new Lazy<PerRequestLifestyle> (() => new PerRequestLifestyle (), true);

		public static PerRequestLifestyle Instance { get { return _instance.Value; } }

		#endregion

		internal static readonly string CallContextKey = (typeof(PerRequestLifestyle)).Name;

		static readonly object ManagerKey = new object ();

		public PerRequestLifestyle () : base ("MasDev.Owin per request lifestyle", true)
		{
		}

		protected override int Length {
			get { return 150; }
		}


		protected override Scope GetCurrentScopeCore (Container container)
		{
			return GetScopeManager (container).CurrentScope;
		}


		protected override Func<Scope> CreateCurrentScopeProvider (Container container)
		{
			var manager = GetScopeManager (container);
			return () => manager.CurrentScope;
		}

		static RequestScopeManager GetScopeManager (Container container)
		{
			var manager = (RequestScopeManager)container.GetItem (ManagerKey);

			if (manager == null) {
				lock (ManagerKey) {
					manager = (RequestScopeManager)container.GetItem (ManagerKey);

					if (manager == null) {
						manager = new RequestScopeManager ();
						container.SetItem (ManagerKey, manager);
					}
				}
			}

			return manager;
		}
	}

	class RequestScopeManager
	{
		public Scope CurrentScope { get; private set; }

		public RequestScopeManager ()
		{
			CurrentScope = CallContext.GetData (PerRequestLifestyle.CallContextKey) as Scope;
		}
	}
}

