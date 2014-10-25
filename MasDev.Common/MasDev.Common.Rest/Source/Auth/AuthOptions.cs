using MasDev.Common.Rest.Auth;


namespace MasDev.Common.Rest.Auth
{
	public class AuthOptions
	{
		public IAuthManager Manager { get; set; }



		public ITokenCompressor TokenCompressor { get; set; }



		public ITokenProtector TokenProtector { get; set; }



		public ITokenSerializer TokenSerializer { get; set; }



		public IExpiration Expiration { get; set; }



		public bool AuthorizeByDefault { get; set; }
	}
}

