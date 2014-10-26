

namespace MasDev.Common.Http
{
	public static class AuthorizationHeader
	{
		public const string Name = Headers.Authorization;
		public const string BearerScheme = "Bearer";
	}





	public static class AuthorizationBearer
	{
		public static string FromHeader (string headerValue)
		{
			return headerValue.Replace (AuthorizationHeader.BearerScheme, string.Empty).Trim ();
		}



		public static string ToHeaderValue (string token)
		{
			return AuthorizationHeader.BearerScheme + "=" + token;
		}
	}
}

