using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using MasDev.Common.Exceptions;


namespace MasDev.Common.Extensions
{
	public static class HttpResponseMessageExtensions
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
	}
}

