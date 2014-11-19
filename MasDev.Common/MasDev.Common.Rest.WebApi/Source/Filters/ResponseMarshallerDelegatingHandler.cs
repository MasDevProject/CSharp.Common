using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using MasDev.Common.IO;
using System.Net;
using System.Net.Http.Headers;


namespace MasDev.Common.Rest.WebApi
{
	public class ResponseMarshallerDelegatingHandler : DelegatingHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync (HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var result = await base.SendAsync (request, cancellationToken);
			if (result.StatusCode == HttpStatusCode.NoContent)
				result.StatusCode = HttpStatusCode.OK;
				
			var objectContent = result.Content as ObjectContent;
			if (objectContent == null)
				return result;

			var value = objectContent.Value;
				
			var redirect = value as Redirect;
			if (redirect != null) {
				result = new HttpResponseMessage (HttpStatusCode.Redirect);
				result.Headers.Location = new Uri (redirect.To);
			}

			var mimed = value as MimedStream;
			if (mimed != null) {
				result = new HttpResponseMessage (HttpStatusCode.OK);
				var content = new StreamContent (mimed.Stream);
				content.Headers.ContentType = new MediaTypeHeaderValue (mimed.Mime);
				result.Content = content;
			} 

			var fileInfo = value as FileInfo;
			if (fileInfo != null)
				result = await FileResult.CreateAsync (fileInfo);

			return result;
		}

	}
}

