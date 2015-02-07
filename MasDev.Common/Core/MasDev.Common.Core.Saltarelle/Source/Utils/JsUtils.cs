
namespace MasDev.Common
{
	public static class JsUtils
	{
		public static bool ContainsProperty (dynamic obj, string property)
		{
			try {
				var v = obj [property];
				return true;
			} catch {
				return false;
			}
		}
	}
}

