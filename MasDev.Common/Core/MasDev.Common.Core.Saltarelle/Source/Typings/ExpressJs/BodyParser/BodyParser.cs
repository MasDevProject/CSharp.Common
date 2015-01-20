using System.Runtime.CompilerServices;

namespace NodeExpress.BodyParser
{
	[Imported, ModuleName (ModuleName), ScriptName ("")]
	public static class BodyParser
	{
		public const string ModuleName = "body-parser";

		[ScriptName ("urlencoded")]
		public static ExpressMiddleware Urlencoded (BodyParserOptions options)
		{
			return null;
		}
	}

	public class BodyParserOptions
	{
		[ScriptName ("extended")]
		public bool Extended;
	}
}

