using MasDev.Utils;
using System;

namespace MasDev.Common
{
	public class Logger : ILogger
	{
		// Console.Writeline is equivalent to native NSLog

		public void Log (string tag, object message)
		{
			Console.WriteLine ("{0}\t{1}", tag, message);
		}

		public void Log (object message)
		{
			Console.WriteLine (message);
		}
	}
}