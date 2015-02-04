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
		public Registry (string registryName)
		{
			//TODO vedere se si può fare altrimenti ignorare il registryname
		}

		public void Put (string key, object value)
		{
			NSUserDefaults.StandardUserDefaults.SetString (JsonConvert.SerializeObject (value), key);
		}

		public T Read<T> (string key, T defaultValue)
		{
			var value = NSUserDefaults.StandardUserDefaults.StringForKey(key);
			return string.IsNullOrEmpty (value) ? defaultValue : JsonConvert.DeserializeObject<T> (value);
		}

		public void Remove (string key)
		{
			NSUserDefaults.StandardUserDefaults.RemoveObject (key);
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
			NSUserDefaults.StandardUserDefaults.Synchronize ();
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
				NSUserDefaults.StandardUserDefaults.ToDictionary ().Keys.ToList ();
			}
		}
	}
}