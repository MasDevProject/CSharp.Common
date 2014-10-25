using System;
using MasDev.Common.IO;
using Android.Content;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MasDev.Common.Droid.IO
{
	public class RegistryProvider : IRegistryProvider 
	{
		readonly Context _context;

		public RegistryProvider(Context context) 
		{
			_context = context;
		}

		public IRegistry GetRegistry (string registryName)
		{
			return new Registry (_context, registryName);
		}
	}

	public class Registry : IRegistry
	{
		readonly ISharedPreferences _manager;
		ISharedPreferencesEditor _editor;
		const string DefaultString = "_ds_";

		public Registry(Context context, string fileName)
		{
			_manager = context.GetSharedPreferences (fileName, FileCreationMode.Private);
			_editor = _manager.Edit ();
		}

		public void Put (string key, object value)
		{
			_editor.PutString (key, JsonConvert.SerializeObject (value));	
		}

		public T Read<T> (string key, T defaultValue)
		{
			var s = _manager.GetString (key, DefaultString);
			return s == DefaultString ? defaultValue : JsonConvert.DeserializeObject<T> (s);
		}

		public void Remove (string key)
		{
			_editor.Remove (key);
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
			//Se ti da errore, prova a riassegnare l'editor sotto questa istruzione.
			_editor.Commit ();
		}

		public Task CommitAsync ()
		{
			_editor.Commit ();
			return TaskUtils.Void;
		}

		public void Rollback ()
		{
			throw new NotSupportedException ("Android SharedPreferences do not have Rollback method");
		}

		public void Clear ()
		{
			_editor.Clear ();
		}

		public RegistryConfiguration Configuration {
			set {
				throw new NotSupportedException ("Android SharedPreferecens do not support Configuration");
			}
		}

		public System.Collections.Generic.IEnumerable<string> Keys {
			get {
				return _manager.All.Keys;
			}
		}
	}
}

