using System;


namespace MasDev.Services.Modeling
{
	public class LoginResult<T>
	{
		public T Identity { get; set; }

		public string AccessToken { get; set; }

		public DateTime AccessTokenExpirationUtc { get; set; }
	}
}

