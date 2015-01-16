using MasDev.Rest.Auth;
using System;


namespace MasDev.Rest.Auth
{
	public class Token
	{
		public ICredentials Credentials { get; set; }



		public DateTime ExpiresUtc { get; set; }



		public int Scope { get; set; }
	}
}

