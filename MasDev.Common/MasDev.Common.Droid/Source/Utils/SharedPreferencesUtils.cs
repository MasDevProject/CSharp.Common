using MasDev.Common.Droid.Utils;
using Android.Content;
using System.Collections.Generic;

namespace MasDev.Common.Droid.Utils
{
	public static class SharedPreferencesUtils
	{
		static ISharedPreferences _manager;
		public static ISharedPreferences Manager { 
			get {
				if (_manager != null)
					return _manager;
				_manager = ApplicationUtils.Context.GetSharedPreferences ("QW", FileCreationMode.Private);
				return _manager;
			}
		}

		#region Writing

		public static void WriteString(string key, string value)
		{
			var editor = Manager.Edit (); 
			editor.PutString (key, value);
			editor.Apply();
		}

		public static void WriteBool(string key, bool value)
		{
			var editor = Manager.Edit (); 
			editor.PutBoolean (key, value);
			editor.Apply();
		}

		public static void WriteFloat(string key, float value)
		{
			var editor = Manager.Edit (); 
			editor.PutFloat (key, value);
			editor.Apply();
		}

		public static void WriteInt(string key, int value)
		{
			var editor = Manager.Edit (); 
			editor.PutInt (key, value);
			editor.Apply();
		}

		public static void WriteLong(string key, long value)
		{
			var editor = Manager.Edit (); 
			editor.PutLong (key, value);
			editor.Apply();
		}

		public static void WriteStringSet(string key, ICollection<string> strings)
		{
			var editor = Manager.Edit (); 
			editor.PutStringSet (key, strings);
			editor.Apply();
		}

		public static void WriteStringIntoStringSet(string key, string stringToAdd)
		{
			var strings = Manager.GetStringSet (key, null) ?? new List<string> ();
			strings.Add (stringToAdd);
			var editor = Manager.Edit (); 
			editor.PutStringSet (key, strings);
			editor.Apply();
		}

		public static void Delete(string key) 
		{
			Manager.Edit ().Remove (key).Apply ();
		}

		public static void DeleteAll() 
		{
			Manager.Edit ().Clear ().Apply ();
		}

		#endregion

		#region Reading

		public static string ReadString(string key, string defValue)
		{
			return Manager.GetString (key, defValue);
		}

		public static bool ReadBool(string key, bool defValue)
		{
			return Manager.GetBoolean (key, defValue);
		}

		public static float ReadFloat(string key, float defValue)
		{
			return Manager.GetFloat (key, defValue);
		}

		public static int ReadInt(string key, int defValue)
		{
			return Manager.GetInt (key, defValue);
		}

		public static long ReadLong(string key, long defValue)
		{
			return Manager.GetLong (key, defValue);
		}

		public static ICollection<string> ReadStringSet(string key, ICollection<string> defValues)
		{
			return Manager.GetStringSet (key, defValues);
		}

		#endregion
	}
}

