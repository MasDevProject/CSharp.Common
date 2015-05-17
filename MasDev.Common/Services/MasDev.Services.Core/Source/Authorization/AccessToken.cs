using System;
using MasDev.Services.Modeling;


namespace MasDev.Services.Auth
{
	public class AccessToken
	{
		public Identity Identity { get; set; }

		public DateTime CreationUtc { get; set; }

		public DateTime ExpirationUtc { get; set; }

		public int? Scope { get; set; }
	}
}

