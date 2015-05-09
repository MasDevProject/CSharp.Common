namespace MasDev.Owin.Auth
{
	public class AccessTokenPipeline
	{
		public IAccessTokenConverter Converter { get; private set; }

		public IAccessTokenProtector Protector { get; private set; }

		public IAccessTokenCompressor Compressor { get; private set; }

		public AccessTokenPipeline (IAccessTokenConverter converter, IAccessTokenProtector protector, IAccessTokenCompressor compressor)
		{
			converter.ThrowIfNull ("converter");
			protector.ThrowIfNull ("protector");
			compressor.ThrowIfNull ("compressor");
			Converter = converter;
			Protector = protector;
			Compressor = compressor;
		}
	}

	public class DefaultAccessTokenPipeline : AccessTokenPipeline
	{
		public DefaultAccessTokenPipeline (string password) : base (new DefaultAccessTokenConverter (), new DefaultAccessTokenProtector (password), new DefaultAccessTokenCompressor ())
		{
		}
	}
}

