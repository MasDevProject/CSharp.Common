using Foundation;


namespace MasDev.iOS.Utils
{
	public static class NSUserDefaultsUtils
	{
		public static NSUserDefaults Manager
		{
			get { return NSUserDefaults.StandardUserDefaults; }
		}

		#region Writing

		public static void WriteBool(string key, bool value)
		{
			Manager.SetBool (value, key);
		}

		public static void WriteDouble(string key, double value)
		{
			Manager.SetDouble (value, key);
		}

		public static void WriteFloat(string key, float value)
		{
			Manager.SetFloat (value, key);
		}

		public static void WriteInt(string key, int value)
		{
			Manager.SetInt (value, key);
		}

		public static void WriteString(string key, string value)
		{
			Manager.SetString (value, key);
		}

		public static void AddExistingDefaults(NSDictionary value)
		{
			Manager.RegisterDefaults (value);
		}

		public static void Commit()
		{
			Manager.Synchronize ();
		}

		#endregion

		#region Reading

		public static bool ReadBool(string key)
		{
			return Manager.BoolForKey(key);
		}

		public static double ReadDouble(string key)
		{
			return Manager.DoubleForKey(key);
		}

		public static float ReadFloat(string key)
		{
			return Manager.FloatForKey(key);
		}

		public static int ReadInt(string key)
		{
			return (int)Manager.IntForKey(key);
		}

		public static string ReadString(string key)
		{
			return Manager.StringForKey(key);
		}

		public static NSObject[] ReadArray(string key)
		{
			return Manager.ArrayForKey (key);
		}

		public static NSDictionary ReadDictionary(string key)
		{
			return Manager.DictionaryForKey(key);
		}

		#endregion

		#region Domain CRUD

		public static string[] DomainNames
		{
			get { return Manager.PersistentDomainNames (); }
		}

		public static void RemoveDomain(string domainName)
		{
			Manager.RemovePersistentDomain (domainName);
		}

		public static void UpdateDomain(string key, NSDictionary value)
		{
			Manager.SetPersistentDomain (value, key);
		}

		public static NSDictionary ReadDomain(string key)
		{
			return Manager.PersistentDomainForName (key);
		}

		#endregion
	}
}