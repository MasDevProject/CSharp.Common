﻿using System.Collections.Generic;
using System.Linq;


namespace MasDev.Rest.Push.ConnectionManagers
{
	public class InMemoryConnectionsCache<T>
	{
		readonly Dictionary<T, HashSet<string>> _connections = new Dictionary<T, HashSet<string>> ();



		public int Count {
			get {
				return _connections.Count;
			}
		}



		public void Add (T key, string connectionId)
		{
			lock (_connections) {
				HashSet<string> connections;
				if (!_connections.TryGetValue (key, out connections)) {
					connections = new HashSet<string> ();
					_connections.Add (key, connections);
				}

				lock (connections) {
					connections.Add (connectionId);
				}
			}
		}



		public IEnumerable<string> GetConnections (T key)
		{
			HashSet<string> connections;
			if (_connections.TryGetValue (key, out connections)) {
				return connections;
			}

			return Enumerable.Empty<string> ();
		}



		public void Remove (T key, string connectionId)
		{
			lock (_connections) {
				HashSet<string> connections;
				if (!_connections.TryGetValue (key, out connections)) {
					return;
				}

				lock (connections) {
					connections.Remove (connectionId);

					if (connections.Count == 0) {
						_connections.Remove (key);
					}
				}
			}
		}


		public void Remove (string connectionId)
		{
			lock (_connections) {
				var keys = _connections.Where (v => v.Value.Contains (connectionId)).Select (v => v.Key);
				foreach (var key in keys)
					_connections.Remove (key);
			}
		}
	}
}

