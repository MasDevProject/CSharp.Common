using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using MasDev.Exceptions;


namespace MasDev.Extensions
{
	public static class HttpExtensions
	{
		public static async Task<T> AsAsync<T> (this HttpResponseMessage msg)
		{
			var s = await msg.Content.ReadAsStringAsync ();
			return JsonConvert.DeserializeObject<T> (s);
		}



		public static void ThrowIfNotOk (this HttpResponseMessage msg)
		{
			if (msg.StatusCode != HttpStatusCode.OK)
				throw new HttpException (msg.StatusCode);
		}

		public static string GetHost (this HttpRequestMessage msg)
		{
			var uri = msg.RequestUri;
			var host = uri.Host;
			return host + ":" + uri.Port + "/";
		}
	}
}

