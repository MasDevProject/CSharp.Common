using NodeJS;

namespace MasDev.Common.Saltarelle
{
	public static class OsUtils
	{
		public static bool IsWindows ()
		{
			return Process.Platform.ToLowerCase ().StartsWith ("win");
		}
	}
}

