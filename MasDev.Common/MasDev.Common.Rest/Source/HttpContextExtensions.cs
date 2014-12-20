using System;
using System.Linq;

namespace MasDev.Common.Rest
{
	public static class HttpContextExtensions
	{
		public static string AcceptLanguageHeader (this IHttpContext context)
		{
			var headers = context.RequestHeaders;
			if (headers == null || !headers.ContainsKey (Http.AcceptLanguageHeader.Name))
				return null;


			var acceptLanguageHeader = headers [Http.AcceptLanguageHeader.Name];
			return acceptLanguageHeader == null ? null : acceptLanguageHeader.FirstOrDefault ();
		}
	}
}

