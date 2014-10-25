using System;
using Microsoft.AspNet.SignalR;
using MasDev.Common.Rest.Push;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Hubs;


namespace MasDev.Common.Rest.SignalR
{
	public abstract class HubPushHttpContext : Hub, IPushHttpContext
	{
		const string _remoteIpAddressKey = "server.RemoteIpAddress";
		readonly Lazy<IPushCallers> _clients;



		protected HubPushHttpContext ()
		{
			_clients = new Lazy<IPushCallers> (() => new HubCallerConnectionContextWrapper (Clients));
		}



		IPushCallers IPushHttpContext.Clients { get { return _clients.Value; } }



		public string ConnectionId { get { return Context.ConnectionId; } }



		public string RemoteIpAddress
		{
			get {
				var environment = Context.Request.Environment;
				if (environment == null)
					throw new Exception ("Could not determine remote ip address");
				return Get<string> (environment, _remoteIpAddressKey);
			}
		}



		public Dictionary<string, IEnumerable<string>> RequestHeaders
		{
			get {
				var dict = new Dictionary<string, IEnumerable<string>> ();
				foreach (var name in Context.Request.Headers)
				{
					var key = name.Key;
					if (dict.ContainsKey (key))
						((List<string>)dict [key]).Add (name.Value);
					else
						dict.Add (key, new List<string> { name.Value });
				}
				return dict;
			}
		}



		public Dictionary<string, IEnumerable<string>> ResponseHeaders
		{
			get {
				throw new NotSupportedException ("Not supported with SignalR");
			}
			set {
				throw new NotSupportedException ("Not supported with SignalR");
			}
		}



		static T Get<T> (IDictionary<string, object> env, string key)
		{
			object value;
			return env.TryGetValue (key, out value) ? (T)value : default(T);
		}
	}





	class HubCallerConnectionContextWrapper : IPushCallers
	{
		readonly IHubCallerConnectionContext<dynamic> _wrapped;



		public HubCallerConnectionContextWrapper (IHubCallerConnectionContext<dynamic> wrapped)
		{
			_wrapped = wrapped;
		}



		public dynamic AllExcept (params string[] excludeConnectionIds)
		{
			return _wrapped.AllExcept (excludeConnectionIds);
		}



		public dynamic Client (string connectionId)
		{
			return _wrapped.Client (connectionId);
		}



		public dynamic Group (string groupName, params string[] excludeConnectionIds)
		{
			return _wrapped.Group (groupName, excludeConnectionIds);
		}



		public dynamic Groups (IList<string> groupNames, params string[] excludeConnectionIds)
		{
			return _wrapped.Groups (groupNames, excludeConnectionIds);
		}



		public dynamic OthersInGroup (string groupName)
		{
			return _wrapped.OthersInGroup (groupName);
		}



		public dynamic OthersInGroups (IList<string> groupNames)
		{
			return _wrapped.OthersInGroups (groupNames);
		}



		public dynamic All { get { return _wrapped.All; } }



		public dynamic Caller { get { return _wrapped.Caller; } }



		public dynamic Others { get { return _wrapped.Others; } }
	}





	class HubConnectionContextWrapper : IPushClients
	{
		readonly IHubConnectionContext<dynamic> _wrapped;



		public HubConnectionContextWrapper (IHubConnectionContext<dynamic> wrapped)
		{
			_wrapped = wrapped;
		}



		public dynamic AllExcept (params string[] excludeConnectionIds)
		{
			return _wrapped.AllExcept (excludeConnectionIds);
		}



		public dynamic Client (string connectionId)
		{
			return _wrapped.Client (connectionId);
		}



		public dynamic Group (string groupName, params string[] excludeConnectionIds)
		{
			return _wrapped.Group (groupName, excludeConnectionIds);
		}



		public dynamic Groups (IList<string> groupNames, params string[] excludeConnectionIds)
		{
			return _wrapped.Groups (groupNames, excludeConnectionIds);
		}





		public dynamic All { get { return _wrapped.All; } }
	}
}

