using System.Web.Http.Filters;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http.Controllers;
using System;
using System.Diagnostics;
using System.Linq;
using MasDev.IO.Http;


namespace MasDev.Rest.WebApi
{
	public class ActionFilter : IActionFilter
	{
		public async Task<HttpResponseMessage> ExecuteActionFilterAsync (HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
		{

			var host = actionContext.ControllerContext.Controller as IHttpContext;
			if (host == null)
				return await continuation ();

			ExceptionLoggerFilter.WriteLoggingHeader (actionContext);
			var stopwatch = Stopwatch.StartNew ();
			var response = await continuation ();
			Debug.WriteLine ("Elapsed: " + stopwatch.ElapsedMilliseconds + " ms");

			Debug.WriteLine ("Response:");
			Debug.WriteLine ("\tStatusCode: " + response.StatusCode);
			if (response.Content != null)
				Debug.WriteLine ("\tContentType: " + response.Content.Headers.ContentType);

			if (host.ResponseHeaders == null || !host.ResponseHeaders.Any ())
				return response;

			Debug.WriteLine ("\tAdditionalHeaders:");
			return AddHeaders (host, response);
		}



		static HttpResponseMessage AddHeaders (IHttpContext host, HttpResponseMessage response)
		{
			foreach (var header in host.ResponseHeaders) {
				switch (header.Key) {
				case Headers.LastModified:
					if (response.Content == null || !header.Value.Any ())
						continue;

					response.Content.Headers.LastModified = DateTimeOffset.Parse (header.Value.FirstOrDefault ());
					continue;
				}

				// DEFAULT HEADERS
				response.Headers.TryAddWithoutValidation (header.Key, header.Value);
				Debug.Write ("\t\t" + header.Key + " ");
				foreach (var value in header.Value)
					Debug.Write (value + "; ");
				Debug.Write ("\n");
			}
			return response;
		}



		public bool AllowMultiple { get { return true; } }

	}
}

