

namespace MasDev.Common.Rest.Auth
{
	public class AuthManagerOptions
	{
		public ITokenCompressor TokenCompressor { get; set; }



		public ITokenProtector TokenProtector { get; set; }



		public ITokenSerializer TokenSerializer { get; set; }



		public IExpiration Expiration { get; set; }
	}
}

