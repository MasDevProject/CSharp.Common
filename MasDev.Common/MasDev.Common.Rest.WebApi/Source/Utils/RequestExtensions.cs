using System.Net.Http;
using System.Linq;


namespace MasDev.Common.Rest.WebApi
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
	}
}

