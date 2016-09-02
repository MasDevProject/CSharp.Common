using Foundation;

namespace MasDev.iOS.Utils
{
	public static class NSBundleUtils
	{
		public static string LocalizedString(string key)
		{
			return NSBundle.MainBundle.LocalizedString (key, string.Empty);
		}

		public static string LocalizedString(string tableName, string key, string defaultValue)
		{
			return NSBundle.MainBundle.LocalizedString (key, defaultValue, tableName);
		}
	}
}