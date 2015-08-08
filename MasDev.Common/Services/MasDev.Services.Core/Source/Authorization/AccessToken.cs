using System;
using MasDev.Services.Modeling;


namespace MasDev.Services.Auth
{
	public interface IAccessToken
	{
		Identity Identity { get; set; }

		DateTime CreationUtc { get; set; }

		DateTime ExpirationUtc { get; set; }

		int? Scope { get; set; }

		int? Extra { get; set; }
	}

	public sealed class AccessToken : IAccessToken
	{
		public Identity Identity { get; set; }

		public DateTime CreationUtc { get; set; }

		public DateTime ExpirationUtc { get; set; }

		public int? Scope { get; set; }

		public int? Extra { get; set; }
	}
}

