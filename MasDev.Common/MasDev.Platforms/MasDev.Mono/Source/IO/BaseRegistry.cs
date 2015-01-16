using System.Collections.Generic;
using MasDev.Security;
using System;
using System.Threading.Tasks;
using MasDev.Extensions;
using MasDev.IO;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using MasDev.Data;


namespace MasDev.IO
{
	public class RegistryProvider : IRegistryProvider
	{
		public IRegistry GetRegistry (string registryName)
		{
			return new BaseRegistry (registryName);
		}
	}





	class BaseRegistry : IRegistry
	{
		readonly IFileIO _file = new FileIO ();


		RegistryConfiguration _configuration;
		List<RegistryEntry> _persistentEntries;
		readonly List<RegistryEntry> _uncommittedChanges;
		readonly List<string> _removedChanges;
		readonly string _registryLocation;
		bool _clear;



		public BaseRegistry (string registryLocation)
		{
			_registryLocation = registryLocation;
			_uncommittedChanges = new List<RegistryEntry> ();
			_removedChanges = new List<string> ();
		}




		public void Prepare ()
		{
			try {
				var rawContent = _file.ReadString (_registryLocation);
				ReadPersistentEntries (rawContent);
				_clear = false;
			} catch (FileNotFoundException) {
				_persistentEntries = new List<RegistryEntry> ();
			}
		}



		public async Task PrepareAsync ()
		{
			try {
				var rawContent = await _file.ReadStringAsync (_registryLocation);
				ReadPersistentEntries (rawContent);
				_clear = false;
			} catch (Exception) {
				_persistentEntries = new List<RegistryEntry> ();
			}
		}



		public void Put (string key, object value)
		{
			if (_clear)
				return;

			var index = _uncommittedChanges.FindFirstIndex (e => e.Key == key);
			var entry = new RegistryEntry{ Key = key, Value = value };
			if (index >= 0)
				_uncommittedChanges [index] = entry;
			else
				_uncommittedChanges.Add (entry);
		}



		public T Read<T> (string key, T defaultValue)
		{
			EnsurePrepareCalled ();
			var index = _persistentEntries.FindFirstIndex (e => e.Key == key);
			var ret = index >= 0 ? (T)_persistentEntries [index].Value : defaultValue;
			return ret;
		}



		public void Commit ()
		{
			if (_clear) {
				if (_file.Exists (_registryLocation))
					_file.Delete (_registryLocation);
				return;
			}
			var serialized = PrepareCommit ();	
			_file.WriteAll (serialized, _registryLocation);
			Rollback ();
		}



		public async Task CommitAsync ()
		{
			if (_clear) {
				if (_file.Exists (_registryLocation))
					_file.Delete (_registryLocation);
				return;
			}
			var serialized = PrepareCommit ();	
			await _file.WriteAllAsync (serialized, _registryLocation);
			Rollback ();
		}



		public void Rollback ()
		{
			_clear = false;
			_uncommittedChanges.Clear ();
			_removedChanges.Clear ();
		}



		public void Remove (string key)
		{
			if (_clear)
				return;
			_removedChanges.Add (key);
		}



		public RegistryConfiguration Configuration{ set { _configuration = value; } }



		void ReadPersistentEntries (string rawContent)
		{
			if (_configuration != null) {
				if (_configuration.Encrypt) {
					EnsureEncryptionKey ();
					rawContent = Aes.StaticDecrypt (rawContent, _configuration.EncryptionKey);
				}
			}
			var x = JsonConvert.DeserializeObject<RegistryEntry[]> (rawContent);
			_persistentEntries = new List<RegistryEntry> (x);
		}



		void EnsurePrepareCalled ()
		{
			if (_persistentEntries == null)
				throw new NotSupportedException ("You must call Perepare or PrepareAsync first");
		}



		void EnsureEncryptionKey ()
		{
			if (string.IsNullOrEmpty (_configuration.EncryptionKey))
				throw new Exception ("You must set EncryptionKey in RegistryConfiguration in order to use Encryption");
		}



		string PrepareCommit ()
		{
			EnsurePrepareCalled ();
			foreach (var entry in _uncommittedChanges) {
				var index = _persistentEntries.FindFirstIndex (e => e.Key == entry.Key);
				if (index >= 0)
					_persistentEntries [index] = entry;
				else
					_persistentEntries.Add (entry);
			}

			foreach (var key in _removedChanges) {
				var index = _persistentEntries.FindFirstIndex (e => e.Key == key);
				if (index >= 0)
					_persistentEntries.RemoveAt (index);
			}
	

			string text = JsonConvert.SerializeObject (_persistentEntries.ToArray ());

			if (_configuration != null) {
				if (_configuration.Encrypt) {
					EnsureEncryptionKey ();
					text = Aes.StaticEncrypt (text, _configuration.EncryptionKey);
				}
			}

			return text;
		}



		public IEnumerable<string> Keys {
			get {
				var i1 = _persistentEntries.Select (entry => entry.Key);
				var i2 = _uncommittedChanges.Select (entry => entry.Key);
				return i1.Concat (i2.Where (k2 => i1.All (k1 => k1 != k2)));
			}
		}



		public void Clear ()
		{

		}
	}





	class RegistryEntry
	{
		public string Key { get; set; }



		public object Value { get; set; }
	}
}

