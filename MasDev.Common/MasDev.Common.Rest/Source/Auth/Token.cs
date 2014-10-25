using MasDev.Common.Rest.Auth;
using System;


namespace MasDev.Common.Rest.Auth
{
	public class Token
	{
		public ICredentials Credentials { get; set; }



		public DateTime ExpiresUtc { get; set; }



		public int Scope { get; set; }
	}
}

