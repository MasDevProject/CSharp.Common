using System;


namespace MasDev.Common.Exceptions
{
	public class ShouldNeverHappenException : Exception
	{
		public ShouldNeverHappenException (string message) : base (message)
		{
		}
	}
}

