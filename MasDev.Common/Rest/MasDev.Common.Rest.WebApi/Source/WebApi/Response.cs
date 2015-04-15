using System.Net.Http;
using System.Text;

namespace MasDev.Common.Rest
{
	public static class Response
	{
		public static HttpResponseMessage Html (string content, Encoding encoding = null)
		{
			var resp = new HttpResponseMessage();
			resp.Content = new StringContent (content, encoding ?? Encoding.UTF8, "text/html");
			return resp;
		}
	}
}

