
namespace MasDev.Services.Auth
{
	public class DefaultAccessTokenPipeline : AccessTokenPipeline
	{
		public DefaultAccessTokenPipeline (string password) : base (new DefaultAccessTokenConverter (), new DefaultAccessTokenProtector (password), new DefaultAccessTokenCompressor ())
		{
		}
	}
}

