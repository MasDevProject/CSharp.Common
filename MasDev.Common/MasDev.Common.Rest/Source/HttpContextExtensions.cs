using System;
using System.Linq;

namespace MasDev.Rest
{
	public static class HttpContextExtensions
	{
		public static string AcceptLanguageHeader (this IHttpContext context)
		{
			var headers = context.RequestHeaders;
			if (headers == null || !headers.ContainsKey (MasDev.IO.Http.AcceptLanguageHeader.Name))
				return null;


			var acceptLanguageHeader = headers [MasDev.IO.Http.AcceptLanguageHeader.Name];
			return acceptLanguageHeader == null ? null : acceptLanguageHeader.FirstOrDefault ();
		}
	}
}

