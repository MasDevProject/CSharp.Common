using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using MasDev.IO;
using System.Net;
using System.Net.Http.Headers;
using AutoMapper;


namespace MasDev.Services.Owin.WebApi
{
	public class SerializerDelegatingHandler : DelegatingHandler
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
				var content = new StreamContent (mimed.Stream);
				Mapper.DynamicMap (result.Content.Headers, content.Headers);
				content.Headers.ContentType = new MediaTypeHeaderValue (mimed.Mime);
				result.Content = content;
			} 

			var fileInfo = value as FileInfo;
			if (fileInfo != null) {
				var ifModifiedSince = request.GetIfModifiedSinceHeader ();
				result = ifModifiedSince == null ? 
					await FileResult.CreateAsync (null, fileInfo) : 
					await FileResult.CreateAsync (ifModifiedSince, fileInfo);
			}

			return result;
		}

	}
}

