using System;

namespace MasDev.Rest
{
	[AttributeUsage (
		System.AttributeTargets.Method,
		AllowMultiple = false,
		Inherited = true
	)]
	public class HandleTransactionsAttribute : Attribute
	{
		public HandleTransactionsAttribute ()
		{
		}
	}
}

