using System;
using System.Linq;

namespace MasDev.Rest
{
	public static class HttpContextExtensions
	{
		public static string AcceptLanguageHeader (this IHttpContext context)
		{
			var headers = context.RequestHeaders;
			if (headers == null || !headers.ContainsKey (MasDev.IO.Http.Headers.AcceptLanguage))
				return null;


			var acceptLanguageHeader = headers [MasDev.IO.Http.Headers.AcceptLanguage];
			return acceptLanguageHeader == null ? null : acceptLanguageHeader.FirstOrDefault ();
		}
	}
}

