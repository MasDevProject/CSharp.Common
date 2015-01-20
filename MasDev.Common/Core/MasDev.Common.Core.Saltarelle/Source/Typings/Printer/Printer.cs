using System.Runtime.CompilerServices;

namespace NodePrinter
{
	[Imported, ModuleName (Printer.ModuleName), ScriptName ("")]
	public class Printer
	{
		public const string ModuleName = "printer";

		public PrinterObject[] GetPrinters ()
		{
			return null;
		}
	}


	public class PrinterObject
	{
		public string Name;
		public bool IsDefault;
		public string Status;
		public dynamic Options;
	}
}

