using Foundation;

namespace MasDev.iOS.Utils
{
	public static class NSBundleUtils
	{
		public static string LocalizedString(string key, string comment = null)
		{
			return NSBundle.MainBundle.LocalizedString (key, comment ?? string.Empty);
		}
	}
}