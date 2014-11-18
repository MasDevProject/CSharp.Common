using System;


namespace MasDev.Common.Rest.Auth
{
	[AttributeUsage (
		System.AttributeTargets.Method,
		AllowMultiple = false,
		Inherited = true
	)]
	public class AuthorizeAttribute : Attribute
	{
		public int? Roles { get; private set; }



		public AuthorizeAttribute (int roles)
		{
			Roles = roles;
		}



		public AuthorizeAttribute ()
		{

		}
	}
}

