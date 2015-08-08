using System.Net.Http;
using System.Text;
using System.Linq;
using MasDev.IO.Http;
using MasDev.Common;
using MasDev.Common.Http;
using System.Collections.Generic;
using System;
using System.Collections.Specialized;
using System.Web;
using MasDev.Extensions;
using System.Threading.Tasks;
using System.IO;
using MasDev.Utils;

namespace MasDev.Services.Owin.WebApi
{
	public static class HttpUtils
	{
		public static HttpResponseMessage AsHtml (this string content, Encoding encoding = null)
		{
			var resp = new HttpResponseMessage ();
			resp.Content = new StringContent (content, encoding ?? Encoding.UTF8, "text/html");
			return resp;
		}

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

		static HttpResponseMessage AddHeaders (HttpResponseMessage response, Dictionary<string, IEnumerable<string>> responseHeaders)
		{
			if (responseHeaders == null || responseHeaders.Count == 0)
				return response;

			foreach (var header in responseHeaders) {
				response.Headers.Add (header.Key, header.Value);
			}
			return response;
		}


		public static DynamicDictionary ParseParameters (HttpContent content)
		{
			if (content.IsMimeMultipartContent ())
				throw new ArgumentException ("Request body must be form urlencoded");

			var rawTask = content.ReadAsStringAsync ();
			rawTask.Wait ();

			if (rawTask.Exception != null)
				throw rawTask.Exception;

			var dict = new DynamicDictionary ();

			var raw = ParseBodyParameters (rawTask.Result);
			raw.AllKeys
				.SelectMany (raw.GetValues, (k, v) => new {Key = k, Value = v})
				.ForEach (el => dict.Add (el.Key, el.Value));

			return dict;
		}



		public static NameValueCollection ParseBodyParameters (string body)
		{
			return HttpUtility.ParseQueryString (body);
		}


		public static async Task<Tuple<string, string>> RequestBodyAsMimedStream (HttpRequestMessage request)
		{
			var tempFileUrl = Path.GetTempPath () + "/" + GuidGenerator.Generate () + ".tmp";
			using (var stream = new FileStream (tempFileUrl, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
				await request.Content.CopyToAsync (stream);

			var mime = request.Content.Headers.ContentType.MediaType;

			return Tuple.Create (tempFileUrl, mime);
		}
	}
}

