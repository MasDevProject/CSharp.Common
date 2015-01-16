using System;
using System.Web.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using MasDev.Rest;
using HttpMultipartParser;
using System.Web;
using MasDev.Rest.Proxy;
using MasDev.Rest.WebApi;


namespace MasDev.Rest
{
	public class WebApiHttpContext : ApiController, IHttpContext
	{
		const string HttpContext = "MS_HttpContext";
		const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
		const string OwinContext = "MS_OwinContext";
		Dictionary<string, IEnumerable<string>> _responseHeaders = new Dictionary<string, IEnumerable<string>> ();
		readonly Lazy<DynamicDictionary> _lazyBodyParameters;



		public WebApiHttpContext ()
		{
			_lazyBodyParameters = new Lazy<DynamicDictionary> (() => WebApiControllerUtils.ParseParameters (Request.Content));
		}



		public string RemoteIpAddress {
			get {
				// Web-hosting
				if (Request.Properties.ContainsKey (HttpContext)) {
					var ctx = (HttpContextWrapper)Request.Properties [HttpContext];
					if (ctx != null) {
						return ctx.Request.UserHostAddress;
					}
				}

				// Self-hosting
				if (Request.Properties.ContainsKey (RemoteEndpointMessage)) {
					dynamic remoteEndpoint = Request.Properties [RemoteEndpointMessage];
					if (remoteEndpoint != null)
						return remoteEndpoint.Address;
				}

				// Self-hosting using Owin
				if (Request.Properties.ContainsKey (OwinContext)) {
					dynamic owinContext = Request.Properties [OwinContext];
					if (owinContext != null)
						return owinContext.Request.RemoteIpAddress;
				}

				throw new NotSupportedException ("None of the hosting options worked");
			}
		}



		public Dictionary<string, IEnumerable<string>> RequestHeaders {
			get {
				var dict = new Dictionary<string, IEnumerable<string>> ();
				foreach (var entry in Request.Headers)
					dict.Add (entry.Key, entry.Value);
				return dict;
			}
		}



		public Dictionary<string, IEnumerable<string>> ResponseHeaders {
			set {
				_responseHeaders = value;
			}

			get {
				return _responseHeaders;
			}
		}



		public DynamicDictionary BodyParameters { get { return _lazyBodyParameters.Value; } }



		public async Task<MultipartContent> ReadMultipartContentAsync ()
		{
			var parser = new MultipartFormDataParser (await Request.Content.ReadAsStreamAsync ());

			var parsedParameters = parser.Parameters;
			var parameters = new Dictionary<string, string> ();
			if (parsedParameters != null) {
				foreach (var param in parsedParameters)
					parameters.Add (param.Key, param.Value.Data);
			}

			var parsedFiles = parser.Files;
			var files = new List<MultipartFile> ();
			if (parsedFiles != null) {
				foreach (var file in parsedFiles)
					files.Add (new MultipartFile (file.Data, file.Name, file.FileName, file.ContentType));
			}

			return new MultipartContent (new MultipartFormDataCollection (parameters), files);
		}

		public string RequestHost {
			get {
				var uri = Request.RequestUri;
				var host = uri.Host;
				return uri.Scheme + "://" + host + ":" + uri.Port + "/";
			}
		}
	}





	public class WebApiController<TModule> : WebApiHttpContext, IController<TModule>
		where TModule : Module, new()
	{

		TModule _module;



		public WebApiController ()
		{
			_module = RestModuleProxy<TModule>.Create (new TModule ());
			_module.HttpContext = this;
		}



		public TModule Module { get { return _module; } }



		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);

			if (_module == null)
				return;
			_module.Dispose ();
			_module = null;
		}
	}
}




