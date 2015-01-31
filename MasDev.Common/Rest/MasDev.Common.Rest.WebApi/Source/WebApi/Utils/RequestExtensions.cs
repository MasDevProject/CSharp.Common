using System.Net.Http;
using System.Linq;
using MasDev.Common;
using System.Net.Http.Headers;
using MasDev.IO.Http;
using MasDev.Common.Http;


namespace MasDev.Rest.WebApi
{
	public static class RequestExtensions
	{
		public static string GetAcceptLanguageOrDefault (this HttpRequestMessage request, string defaultLocale = "it_IT")
		{
			var acceptLanguage = request.Headers.AcceptLanguage;
			if (acceptLanguage == null)
				return defaultLocale;

			var locale = acceptLanguage.FirstOrDefault ();
			return locale == null ? defaultLocale : locale.Value;
		}


		public static IfModifiedSinceHeader GetIfModifiedSinceHeader (this HttpRequestMessage request)
		{
			var headers = request.Headers;
			if (!headers.Contains (Headers.IfModifiedSince))
				return null;

			var headerValue = headers.GetValues (Headers.IfModifiedSince).FirstOrDefault ();
			return headerValue == null ? null : new IfModifiedSinceHeader (headerValue);
		}

		public static HttpResponseMessage AddHeader (this HttpResponseMessage response, IHeader header)
		{
			var headers = response.Headers;
			headers.TryAddWithoutValidation (header.Name, header.ToString ());
			return response;
		}
	}
}

