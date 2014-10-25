using MasDev.Common.Rest.Auth;
using MasDev.Common.Utils;


namespace MasDev.Common.Rest
{
	public static class RestConfiguration
	{
		static AuthOptions _authOptions;



		public static AuthOptions AuthOptions
		{ 
			get { return _authOptions; }

			set {
				_authOptions = Assert.NotNull (value);
				_authOptions.Manager.Options = new AuthManagerOptions {
					TokenCompressor = Assert.NotNull (value.TokenCompressor),
					TokenProtector = Assert.NotNull (value.TokenProtector),
					TokenSerializer = Assert.NotNull (value.TokenSerializer),
					Expiration = Assert.NotNull (value.Expiration)
				};
			}
		}
	}
}

