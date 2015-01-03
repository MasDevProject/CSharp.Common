using System;
using System.Web.Http;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;
using System.Web;
using System.Net.Http.Headers;
using System.Reflection;


namespace MasDev.Rest.WebApi
{
	public class FileResult : IHttpActionResult
	{
		readonly string _filePath;
		readonly string _contentType;
		readonly HttpStatusCode _statusCode;



		public FileResult (string filePath, string contentType = null)
		{
			if (filePath == null)
				throw new ArgumentNullException ("filePath");

			_filePath = filePath;
			_contentType = contentType;
			_statusCode = HttpStatusCode.OK;
		}

		public FileResult (string filePath, HttpStatusCode statusCode, string contentType = null) : this (filePath, contentType)
		{
			_statusCode = statusCode;
		}



		public Task<HttpResponseMessage> ExecuteAsync (CancellationToken cancellationToken)
		{
			var response = new HttpResponseMessage (HttpStatusCode.OK) {
				Content = new StreamContent (File.OpenRead (_filePath))
			};

			response.StatusCode = _statusCode;
			var contentType = _contentType ?? MimeMapping.GetMimeMapping (Path.GetExtension (_filePath));
			response.Content.Headers.ContentType = new MediaTypeHeaderValue (contentType);

			return Task.FromResult (response);
		}



		public static Task<HttpResponseMessage> CreateAsync (FileInfo fi)
		{
			var result = new FileResult (fi.FullName);
			return result.ExecuteAsync (CancellationToken.None);
		}
	}
}

