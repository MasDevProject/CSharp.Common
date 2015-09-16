using Android.OS;
using Newtonsoft.Json;

namespace MasDev.Droid.ExtensionMethods
{
	public static class BundleExtensions
	{
		public static void PutObject (this Bundle bundle, string key, object @object)
		{
			bundle.PutString (key, JsonConvert.SerializeObject (@object));
		}

		public static T GetObject<T> (this Bundle bundle, string key)
		{
			return JsonConvert.DeserializeObject<T> (bundle.GetString (key));
		}
	}
}

