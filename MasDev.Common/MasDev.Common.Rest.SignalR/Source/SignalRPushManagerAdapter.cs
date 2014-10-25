using MasDev.Common.Rest.Push;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System;
using Microsoft.AspNet.SignalR.Hubs;
using System.Linq;
using MasDev.Common.Utils;
using MasDev.Common.Extensions;
using System.Reflection;


namespace MasDev.Common.Rest.SignalR
{
	public class SignalRPushContextAdapter : IPushContextAdapter
	{
		readonly static Dictionary<Type, string> _map = new Dictionary<Type, string> ();



		public SignalRPushContextAdapter ()
		{
			var callingAssembly = Assembly.GetCallingAssembly ();
			var referencedAssemblies = callingAssembly.LoadRefrencedAssemblies ().Concat (callingAssembly);
			if (referencedAssemblies == null)
				return;
			var types = referencedAssemblies
				.Select (a => a.TryGetExportedTypes () as IEnumerable<Type>)
				.Where (Check.IsNotNull)
				.Aggregate ((acc, curr) => acc == null ? curr : acc.Concat (curr))
				.Where (t => typeof(HubPushHttpContext).IsAssignableFrom (t));

			foreach (var signalRHubType in types)
			{
				var baseType = signalRHubType.BaseType;
				if (!baseType.IsGenericType)
					continue;
				var managerType = baseType.GenericTypeArguments.Single ();
				Assert.True (typeof(PushManager).IsAssignableFrom (managerType));
				Subscribe (signalRHubType, managerType);
			}
		}



		static void Subscribe (Type hubType, Type managerType)
		{
			if (_map.ContainsKey (managerType))
				return;

			var hubName = hubType.Name;
			var hubNameAttribute = hubType.GetCustomAttributes (false).OfType<HubNameAttribute> ().SingleOrDefault ();
			if (hubNameAttribute != null)
				hubName = hubNameAttribute.HubName;

			_map.Add (managerType, hubName);
		}



		public IPushClients Push<T> () where T : PushManager
		{
			var managerType = typeof(T);
			if (!_map.ContainsKey (managerType))
				throw new ArgumentException (managerType.Name + " not registered in SignalR context");

			var hubName = _map [managerType];
			var context = GlobalHost.ConnectionManager.GetHubContext (hubName);
			return new HubConnectionContextWrapper (Assert.NotNull (context).Clients);
		}
	}
}

