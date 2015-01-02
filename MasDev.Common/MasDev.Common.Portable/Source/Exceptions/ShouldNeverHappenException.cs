using System;


namespace MasDev.Exceptions
{
	public class ShouldNeverHappenException : Exception
	{
		public ShouldNeverHappenException (string message) : base (message)
		{
		}
	}
}

