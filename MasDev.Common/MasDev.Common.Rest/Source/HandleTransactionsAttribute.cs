using System;

namespace MasDev.Common.Rest
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

