using System;

namespace MasDev.Services.Auth
{
	public class AccessTokenPipeline
	{
		public IAccessTokenConverter Converter { get; private set; }

		public IAccessTokenProtector Protector { get; private set; }

		public IAccessTokenCompressor Compressor { get; private set; }

		public AccessTokenPipeline (IAccessTokenConverter converter, IAccessTokenProtector protector, IAccessTokenCompressor compressor)
		{
			if (converter == null)
				throw new ArgumentNullException (nameof(converter));
			if (protector == null)
				throw new ArgumentNullException (nameof(protector));
			if (compressor == null)
				throw new ArgumentNullException (nameof(compressor));
			
			Converter = converter;
			Protector = protector;
			Compressor = compressor;
		}
	}
}

