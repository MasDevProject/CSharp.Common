using MasDev.Rest.Push;
using System;
using System.Collections.Generic;
using MasDev.Data;


namespace MasDev.Rest
{
	public static class PushContext
	{
		static IPushContextAdapter _adapter;
		static IConnectionManagerFactory _connectionFactory;

		public static void Use (IPushContextAdapter adapter, IConnectionManagerFactory managers)
		{
			_adapter = adapter;
			_connectionFactory = managers;
		}

		public static IPushClients Push<T> () where T : PushManager
		{
			if (_adapter == null)
				throw new NotSupportedException ("You must use an Adapter and a ConnectionManagerFactory first");

			return _adapter.Push<T> ();
		}

		public static IReadonlyConnectionManager<TModel> ConnectionManager<TModel> (IRepositories repositories) where TModel :class, IModel, new()
		{
			var connectionManager = ConnectionManagerInternal<TModel> (repositories);
			return connectionManager == null ? null : connectionManager.AsReadOnly ();
		}

		internal static IConnectionManager<TModel> ConnectionManagerInternal<TModel> (IRepositories repositories) where TModel :class, IModel, new()
		{
			if (_connectionFactory == null)
				throw new NotSupportedException ("You must use an Adapter and a ConnectionManagerFactory first");

			return _connectionFactory.Create<TModel> (repositories);
		}
	}
}

