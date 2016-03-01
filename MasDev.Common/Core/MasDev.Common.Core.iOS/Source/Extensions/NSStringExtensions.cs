using Foundation;

namespace MasDev.iOS.Extensions
{
	public static class NSStringExtensions
	{
		public static NSString ToNSString(this string value)
		{
			return new NSString (value);
		}
	}
}