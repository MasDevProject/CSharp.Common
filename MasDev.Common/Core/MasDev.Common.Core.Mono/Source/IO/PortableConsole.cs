﻿using System;


namespace MasDev.IO
{
	public class PortableConsole : IPortableConsole
	{
		public void WriteLine (string format, params object[] arguments)
		{
			Console.WriteLine (format, arguments);
		}



		public void WriteLine (object obj)
		{
			Console.WriteLine (obj);
		}
	}
}

