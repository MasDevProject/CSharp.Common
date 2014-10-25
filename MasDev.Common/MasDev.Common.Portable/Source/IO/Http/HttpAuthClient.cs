using System;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using MasDev.Common.IO;
using System.Net.Http.Headers;


namespace MasDev.Common.Http
{
	public abstract class HttpAuthClient : IDisposable
	{
		readonly HttpClient _client;
		readonly string _baseUrl;
		public Dictionary<string, IEnumerable<string>> Headers { get; private set; }


		public abstract string Token { get; set; }



		public abstract string TokenType { get; set; }



		public TimeSpan Timeout
		{ 
			set { _client.Timeout = value; }
			get { return _client.Timeout; }
		}



		protected HttpAuthClient (string protocol, string address, int? port)
		{
			if (protocol == "https" && port != null)
				throw new ArgumentException ("Https only supports port the default port (443)");

			_baseUrl = string.Format ("{0}://{1}", protocol, address);
			if (port != null)
				_baseUrl += (":" + port);
			_baseUrl += "/";

			Headers = new Dictionary<string, IEnumerable<string>> ();

			_client = new HttpClient ();
		}


        protected HttpAuthClient (string protocol, string address, int? port, HttpMessageHandler handler) : this(protocol, address, port)
        {
            _client.Dispose();
            _client = new HttpClient(handler);
        }




		public async Task<HttpResponseMessage> GetAsync (string relativeUrl, bool requiresAuthorization = false, params ParamKeyPair[] args)
		{
			var request = BuildRequest (HttpMethod.Get, relativeUrl, requiresAuthorization, args);
			return await _client.SendAsync (request);
		}



		public async Task<HttpResponseMessage> PostAsync (string relativeUrl, bool requiresAuthorization = false, params ParamKeyPair[] args)
		{
			var request = BuildRequest (HttpMethod.Post, relativeUrl, requiresAuthorization, args);
			return await _client.SendAsync (request);
		}



		public async Task<HttpResponseMessage> PostAsync (string relativeUrl, MimedStream stream, string paramName, bool requiresAuthorization = false)
		{
			var req = new HttpRequestMessage (HttpMethod.Post, _baseUrl + relativeUrl);
			if (requiresAuthorization)
				BuildHeader (req);

			using (var content = new MultipartFormDataContent ())
			using (var imageContent = new StreamContent (stream.Stream))
			{
				content.Add (imageContent, paramName, stream.FileName.Replace (" ", string.Empty));
				imageContent.Headers.ContentType = new MediaTypeHeaderValue (stream.Mime);
				req.Content = content;

				return await _client.SendAsync (req);
			}
		}



		public async Task<HttpResponseMessage> PutAsync (string relativeUrl, bool requiresAuthorization = false, params ParamKeyPair[] args)
		{
			var request = BuildRequest (HttpMethod.Put, relativeUrl, requiresAuthorization, args);
			return await _client.SendAsync (request);
		}



		public async Task<HttpResponseMessage> PutAsync (string relativeUrl, MimedStream stream, string paramName, bool requiresAuthorization = false)
		{
			var req = new HttpRequestMessage (HttpMethod.Put, _baseUrl + relativeUrl);
			if (requiresAuthorization)
				BuildHeader (req);
				
			using (var content = new MultipartFormDataContent ())
			using (var imageContent = new StreamContent (stream.Stream))
			{
				content.Add (imageContent, paramName, stream.FileName.Replace (" ", string.Empty));
				imageContent.Headers.ContentType = new MediaTypeHeaderValue (stream.Mime);
				req.Content = content;

				return await _client.SendAsync (req);
			}
		}



		public async Task<HttpResponseMessage> DeleteAsync (string relativeUrl, bool requiresAuthorization = false, params ParamKeyPair[] args)
		{
			var request = BuildRequest (HttpMethod.Delete, relativeUrl, requiresAuthorization, args);
			return await _client.SendAsync (request);
		}



		public async Task<HttpResponseMessage> HeadAsync (string relativeUrl, bool requiresAuthorization = false, params ParamKeyPair[] args)
		{
			var request = BuildRequest (HttpMethod.Head, relativeUrl, requiresAuthorization, args);
			return await _client.SendAsync (request);
		}



		public void Dispose ()
		{
			_client.Dispose ();
		}

		#region Private methods

		void BuildHeader (HttpRequestMessage request)
		{
			request.Headers.TryAddWithoutValidation (AuthorizationHeader.HeaderName, TokenType + " " + Token);
		}



		HttpRequestMessage BuildRequest (HttpMethod method, string url, bool requiresAuthorization, ParamKeyPair[] content)
		{
			var request = new HttpRequestMessage (method, _baseUrl + url);
			if (requiresAuthorization)
				BuildHeader (request);

			if (content != null && content.Any ())
			{
				if (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Head)
				{
					var formContent = new FormUrlEncodedContent (content.Select (p => new KeyValuePair<string, string> (p.Key, p.Value)));
					request.Content = formContent;
				} else
				{
					var uri = request.RequestUri + "?";
					foreach (var param in content)
						uri = string.Format ("{0}&{1}={2}", uri, param.Key, Uri.EscapeDataString (param.Value));
					request.RequestUri = new Uri (uri);
				}
			}

			AddRequestHeaders (request);

			return request;
		}

		void AddRequestHeaders (HttpRequestMessage request)
		{
			if (!Headers.Any ())
				return;

			foreach (var h in Headers)
				request.Headers.TryAddWithoutValidation (h.Key, h.Value);
		}

		#endregion
	}





	public class ParamKeyPair
	{
		public string Key { get; private set; }



		public string Value { get; private set; }



		public ParamKeyPair (string key, string value)
		{
			Key = key;
			Value = value;
		}
	}
}

