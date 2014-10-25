using System;
using MasDev.Common.Rest.Auth;


namespace MasDev.Common.Rest
{
	public class DefaultAuthOptions : AuthOptions
	{
		static readonly IExpiration _expiration = new DefaultExpiration ();
		static readonly ITokenCompressor _tokenCompressor = new GZipTokenCompressor ();
		static readonly ITokenSerializer _tokenSerializer = new JSonTokenSerializer ();



		public DefaultAuthOptions (string encryptionKey, IExpiration expiration = null)
		{
			Expiration = expiration ?? _expiration;
			TokenCompressor = _tokenCompressor;
			TokenSerializer = _tokenSerializer;
			TokenProtector = new AesTokenProtector (encryptionKey);
			Manager = new BaseAuthManager ();
		}
	}





	class DefaultExpiration : IExpiration
	{
		public TimeSpan GetExpiration (ICredentials credentials)
		{
			return TimeSpan.FromDays (365);
		}
	}
}

