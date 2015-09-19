using Foundation;

namespace MasDev.iOS.Extensions
{
	public static class StringExtensions
	{
		public static NSString ToNSString(this string value)
		{
			return new NSString (value);
		}
	}
}