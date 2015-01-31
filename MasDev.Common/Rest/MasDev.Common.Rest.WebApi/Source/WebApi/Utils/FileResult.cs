using System;
using System.Web.Http;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;
using System.Web;
using System.Net.Http.Headers;
using MasDev.IO.Http;
using MasDev.Extensions;
using MasDev.Common;


namespace MasDev.Rest.WebApi
{
	public class FileResult : IHttpActionResult
	{
		readonly string _filePath;
		readonly string _contentType;
		readonly HttpStatusCode _statusCode;
		readonly DateTime? _ifModifiedSince;

		public FileResult (IfModifiedSinceHeader ifModifiedSince, string filePath, string contentType = null)
		{
			if (filePath == null)
				throw new ArgumentNullException ("filePath");

			_filePath = filePath;
			_contentType = contentType;
			_statusCode = HttpStatusCode.OK;
			_ifModifiedSince = ifModifiedSince == null ? null : (DateTime?)ifModifiedSince.TimeUtc;
		}

		public FileResult (IfModifiedSinceHeader ifModifiedSince, string filePath, HttpStatusCode statusCode, string contentType = null) : this (ifModifiedSince, filePath, contentType)
		{
			_statusCode = statusCode;
		}

		public Task<HttpResponseMessage> ExecuteAsync (CancellationToken cancellationToken)
		{
			var fileInfo = new FileInfo (_filePath);
			if (_ifModifiedSince.HasValue && _statusCode == HttpStatusCode.OK) {
				if (fileInfo.LastWriteTimeUtc <= _ifModifiedSince.Value)
					return Task.FromResult (new HttpResponseMessage (HttpStatusCode.NotModified));
			}
			
			var response = new HttpResponseMessage (HttpStatusCode.OK) {
				Content = new StreamContent (File.OpenRead (_filePath))
			};
					
			response.Content.Headers.LastModified = fileInfo.LastWriteTimeUtc;
			var contentType = _contentType ?? MimeMapping.GetMimeMapping (Path.GetExtension (_filePath));
			response.Content.Headers.ContentType = new MediaTypeHeaderValue (contentType);

			return Task.FromResult (response);
		}


		public static Task<HttpResponseMessage> CreateAsync (IfModifiedSinceHeader ifModifiedSince, FileInfo fi)
		{
			var result = new FileResult (ifModifiedSince, fi.FullName);
			return result.ExecuteAsync (CancellationToken.None);
		}
	}
}

