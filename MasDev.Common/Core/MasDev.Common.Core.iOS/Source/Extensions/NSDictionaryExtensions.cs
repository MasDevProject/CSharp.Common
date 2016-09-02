using Foundation;
using MasDev.iOS.Extensions;

namespace MasDev.Common
{
	public static class NSDictionaryExtensions
	{
		public static bool ContainsKey(this NSDictionary dictionary, string key)
		{
			return dictionary.ContainsKey (key.ToNSString ());
		}
	}
}