using System;


namespace MasDev.Common.Rest.Auth
{
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

