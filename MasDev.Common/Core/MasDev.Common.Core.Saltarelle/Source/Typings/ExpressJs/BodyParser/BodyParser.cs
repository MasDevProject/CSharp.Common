using System.Runtime.CompilerServices;

namespace NodeExpress.BodyParser
{
	[Imported, ModuleName ("body-parser"), ScriptName ("")]
	public class BodyParser
	{
		public static readonly string ModuleName = "body-parser";

		[ScriptName ("urlencoded")]
		public ExpressMiddleware Urlencoded (BodyParserOptions options)
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

