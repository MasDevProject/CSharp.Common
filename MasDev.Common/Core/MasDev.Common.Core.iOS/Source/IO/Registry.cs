using System;
using MasDev.Threading.Tasks;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Foundation;

namespace MasDev.IO
{
	public class RegistryProvider : IRegistryProvider
	{
		public IRegistry GetRegistry (string registryName)
		{
			return new Registry (registryName);
		}
	}

	public class Registry : IRegistry
	{
		// Choose to use only StandardUserDefaults because using Domain
		// we would have to read and write the whole file each time

		NSUserDefaults Manager
		{
			get { return NSUserDefaults.StandardUserDefaults; }
		}

		public Registry (string registryName)
		{
		}

		public void Put (string key, object value)
		{
			Manager.SetString (JsonConvert.SerializeObject (value), key);
		}

		public T Read<T> (string key, T defaultValue)
		{
			var value = Manager.StringForKey(key);
			return string.IsNullOrEmpty (value) ? defaultValue : JsonConvert.DeserializeObject<T> (value);
		}

		public void Remove (string key)
		{
			Manager.RemoveObject (key);
		}

		public void Prepare ()
		{
		}

		public Task PrepareAsync ()
		{
			return TaskUtils.Void;
		}

		public void Commit ()
		{
			Manager.Synchronize ();
		}

		public Task CommitAsync ()
		{
			Commit ();
			return TaskUtils.Void;
		}

		public void Rollback ()
		{
			throw new NotSupportedException ("NSUserDefaults doesn't implement a Rollback feature yet");
		}

		public void Clear ()
		{
			var keys = Keys;
			foreach (var key in keys)
				Remove (key);
			Commit ();
		}

		public RegistryConfiguration Configuration {
			set {
				throw new NotSupportedException ("NSUserDefaults doesn't support Configuration yet");
			}
		}

		public IEnumerable<string> Keys {
			get {
				var keys = new List<string> ();
				var objs = Manager.ToDictionary ().Keys;

				foreach (var key in objs)
					keys.Add (key.ToString ());

				return keys;
			}
		}
	}
}