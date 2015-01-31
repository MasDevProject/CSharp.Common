using MasDev.Common.Http;

namespace MasDev.IO.Http
{
	public class AuthorizationHeader : IHeader
	{
		readonly string _scheme;
		readonly string _token;

		public string Name { get { return Headers.Authorization; } }

		public AuthorizationHeader (string scheme, string token)
		{
			_scheme = scheme;
			_token = token;
		}


		public override string ToString ()
		{
			return string.Format ("{0} = {1}", _scheme, _token);
		}

		public System.Collections.Generic.IEnumerable<string> Values {
			get {
				yield return ToString ();
			}
		}
	}





	public static class AuthorizationBearer
	{
		public const string SchemeName = "Bearer";

		public static string FromHeader (string headerValue)
		{
			return headerValue.Replace (SchemeName, string.Empty).Trim ();
		}



		public static string ToHeaderValue (string token)
		{
			return string.Format ("{0} = {1}", SchemeName, token);
		}
	}
}

