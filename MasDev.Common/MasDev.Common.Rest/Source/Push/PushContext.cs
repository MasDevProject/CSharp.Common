using MasDev.Common.Rest.Push;
using System;
using System.Collections.Generic;


namespace MasDev.Common.Rest
{
	public static class PushContext
	{
		static readonly Dictionary<Type, Func<IRepositories, IConnectionManager>> _connectionManagers = new Dictionary<Type, Func<IRepositories, IConnectionManager>> ();

		static IPushContextAdapter _adapter;



		public static void Use (IPushContextAdapter adapter)
		{
			_adapter = adapter;
		}



		public static IPushClients Push<T> () where T : PushManager
		{
			if (_adapter == null)
				throw new NotSupportedException ("You must use an adapter first");

			return _adapter.Push<T> ();
		}



		internal static void RegisterConnectionManager (Type pusher, Func<IRepositories, IConnectionManager> creator)
		{
			if (_connectionManagers.ContainsKey (pusher))
				return;
			_connectionManagers.Add (pusher, creator);
		}



		public static IReadonlyConnectionManager ConnectionManager<TPushManager> (IRepositories repositories) where TPushManager : PushManager, new()
		{
			var manager = typeof(TPushManager);
			if (!_connectionManagers.ContainsKey (manager))
				return null;

			return _connectionManagers [manager] (repositories).AsReadOnly ();
		}
	}
}

