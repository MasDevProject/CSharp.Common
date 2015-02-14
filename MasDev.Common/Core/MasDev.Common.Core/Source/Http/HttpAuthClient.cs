using System;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using MasDev.IO;
using System.Net.Http.Headers;
using System.Net;
using MasDev.Extensions;
using System.Text;
using MasDev.Utils;
using MasDev.Reflection;
using MasDev.IO.Http;
using Newtonsoft.Json;


namespace MasDev.IO.Http
{
	public enum ContentType {
		FormUrlEncoded,
		Json,
		None
	}

	public abstract class HttpAuthClient : IDisposable
	{
		const string WILDCARD_FORMAT = "{{{0}}}";
		const string APPLICATION_JSON = "application/json";
		readonly HttpClient _client;
		readonly string _baseUrl;



		public Dictionary<string, IEnumerable<string>> Headers { get; private set; }



		public abstract string Token { get; set; }



		public abstract string TokenType { get; set; }



		public TimeSpan Timeout { 
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



		protected HttpAuthClient (string protocol, string address, int? port, HttpMessageHandler handler) : this (protocol, address, port)
		{
			_client.Dispose ();
			_client = new HttpClient (handler);
		}




		public async Task<HttpResponseMessage> GetAsync (string relativeUrl, bool requiresAuthorization = false, params HttpParameter[] args)
		{
			var request = BuildRequest (HttpMethod.Get, relativeUrl, requiresAuthorization, ContentType.None, args);
			return await _client.SendAsync (request);
		}


		public async Task<HttpResponseMessage> PostAsync (string relativeUrl, bool requiresAuthorization = false, ContentType contentType = ContentType.FormUrlEncoded, params HttpParameter[] args)
		{
			var request = BuildRequest (HttpMethod.Post, relativeUrl, requiresAuthorization, contentType, args);
			return await _client.SendAsync (request);
		}



		public async Task<HttpResponseMessage> PostAsync (string relativeUrl, MimedStream stream, string paramName, bool requiresAuthorization = false)
		{
			var req = new HttpRequestMessage (HttpMethod.Post, _baseUrl + relativeUrl);
			if (requiresAuthorization)
				BuildHeader (req);

			using (var content = new MultipartFormDataContent ())
			using (var imageContent = new StreamContent (stream.Stream)) {
				content.Add (imageContent, paramName, stream.FileName.Replace (StringUtils.Space, string.Empty));
				imageContent.Headers.ContentType = new MediaTypeHeaderValue (stream.Mime);
				req.Content = content;

				return await _client.SendAsync (req);
			}
		}



		public async Task<HttpResponseMessage> PutAsync (string relativeUrl, bool requiresAuthorization = false, ContentType contentType = ContentType.FormUrlEncoded, params HttpParameter[] args)
		{
			var request = BuildRequest (HttpMethod.Put, relativeUrl, requiresAuthorization, contentType, args);
			return await _client.SendAsync (request);
		}



		public async Task<HttpResponseMessage> PutAsync (string relativeUrl, MimedStream stream, string paramName, bool requiresAuthorization = false)
		{
			var req = new HttpRequestMessage (HttpMethod.Put, _baseUrl + relativeUrl);
			if (requiresAuthorization)
				BuildHeader (req);
				
			using (var content = new MultipartFormDataContent ())
			using (var imageContent = new StreamContent (stream.Stream)) {
				content.Add (imageContent, paramName, stream.FileName.Replace (StringUtils.Space, string.Empty));
				imageContent.Headers.ContentType = new MediaTypeHeaderValue (stream.Mime);
				req.Content = content;

				return await _client.SendAsync (req);
			}
		}



		public async Task<HttpResponseMessage> DeleteAsync (string relativeUrl, bool requiresAuthorization = false, params HttpParameter[] args)
		{
			var request = BuildRequest (HttpMethod.Delete, relativeUrl, requiresAuthorization, ContentType.None, args);
			return await _client.SendAsync (request);
		}



		public async Task<HttpResponseMessage> HeadAsync (string relativeUrl, bool requiresAuthorization = false, params HttpParameter[] args)
		{
			var request = BuildRequest (HttpMethod.Head, relativeUrl, requiresAuthorization, ContentType.None, args);
			return await _client.SendAsync (request);
		}



		public void Dispose ()
		{
			_client.Dispose ();
		}

		#region Private methods

		void BuildHeader (HttpRequestMessage request)
		{
			request.Headers.TryAddWithoutValidation (Http.Headers.Authorization, TokenType + StringUtils.Space + Token);
		}



		HttpRequestMessage BuildRequest (HttpMethod method, string url, bool requiresAuthorization, ContentType contentType, IEnumerable<HttpParameter> c)
		{
			var content = c == null ? null : c.ToArray () ?? new HttpParameter[0];

			#region ParametersValidation

			var formParameters = content.Where (p => p.ParameterType == ParameterType.Form).ToList ();
			if ((!(method == HttpMethod.Post || method == HttpMethod.Put) || contentType != ContentType.FormUrlEncoded) && formParameters.Any ()) 
				throw new ArgumentException ("Form parameters are allowed only in Post or Put requests");

			var contentParamenters = content.Where (p => p.ParameterType == ParameterType.Content).ToList ();
			if (contentParamenters.Count > 1)
				throw new ArgumentException ("Only a single content paramenter in allowed");

			if ((contentType == ContentType.None || contentType == ContentType.FormUrlEncoded) && contentParamenters.Any ())
				throw new ArgumentException ("Using content parameter implies content type different from None or FormUrlEncoded");

			if (method != HttpMethod.Post || method != HttpMethod.Put && contentParamenters.Any ())
				throw new ArgumentException ("You can use content paramenters in PUT and POST only");

			#endregion

			#region HandlingUrlParameters
			var actualUrl = url;
			var urlParameters = content.Where (p => p.ParameterType == ParameterType.Url);
			foreach (var urlParameter in urlParameters) {
				var parameterWildcard = string.Format (WILDCARD_FORMAT, urlParameter.Key);

				var occurrences = actualUrl.Occurrences (parameterWildcard);
				if (occurrences != 1)
					throw new ArgumentException ("Invalid parameter " + parameterWildcard + " for relative url " + url + ". Only a single instance of " + parameterWildcard + " is allowed");

				actualUrl = actualUrl.Replace (parameterWildcard, WebUtility.UrlEncode (urlParameter.Value));
			}
			#endregion

			#region HandlingQueryParameters
			var queryParameters = content.Where (p => p.ParameterType == ParameterType.Query).ToArray ();
			var builder = new StringBuilder (actualUrl);
			for (var i = 0; i < queryParameters.Length; i++) {
				if (i == 0)
					builder.Append ('?');

				var queryParameter = queryParameters [i];
				builder.Append (queryParameter.Key);
				builder.Append ('=');
				builder.Append (WebUtility.UrlEncode (queryParameter.Value));

				if (i != queryParameters.Length - 1)
					builder.Append ('&');
			}
			actualUrl = builder.ToString ();
			#endregion

			var request = new HttpRequestMessage (method, _baseUrl + actualUrl);
			if (requiresAuthorization)
				BuildHeader (request);

			#region HeaderParametersHandling
			var headers = content.Where (p => p.ParameterType == ParameterType.Header);
			foreach (var header in headers)
				request.Headers.TryAddWithoutValidation (header.Key, header.Value);
			#endregion

			#region FormParametersHandling
			if (formParameters.Any ()) {
				var formContent = new FormUrlEncodedContent (formParameters.Select (p => new KeyValuePair<string, string> (p.Key, p.Value)));
				request.Content = formContent;
			}			
			#endregion

			#region ContentParametersHandling

			if (contentParamenters.Any ()) {
				var contentParamenter = contentParamenters.Single ();
				switch (contentType) {
				case ContentType.Json:
					var jsonContent = new StringContent (contentParamenter.Value, Encoding.UTF8, APPLICATION_JSON);
					request.Content = jsonContent;
					break;
				default: throw new NotSupportedException ("Unsupported content type [" + contentType + "]");
				}
			}

			#endregion

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





	public enum ParameterType
	{
		Query,
		Url,
		Form,
		Header,
		Content
	}





	public class HttpParameter
	{
		public string Key { get; private set; }



		public string Value { get; private set; }



		public ParameterType ParameterType { get; private set; }



		public HttpParameter (string key, string value, ParameterType paramType)
		{
			Key = key;
			Value = value;
			ParameterType = paramType;
		}



		public HttpParameter (string key, object value, ParameterType paramType)
		{
			Key = key;
			Value = SerializationUtils.Serialize (value);
			ParameterType = paramType;
		}
	}





	public class FormHttpParameter : HttpParameter
	{
		public FormHttpParameter (string key, object value) : base (key, value, ParameterType.Form)
		{
		}
	}





	public class QueryHttpParameter : HttpParameter
	{
		public QueryHttpParameter (string key, object value) : base (key, value, ParameterType.Query)
		{
			Assert.NotNull (value);

			if (Types.IsNativeType (value))
				return;

			try {
				var values = Enum.GetValues (value.GetType ());
				if (values != null)
					return;
			} catch (ArgumentException) {

			}

			throw new ArgumentException ("Only native types are allowed for this kind of parameter");
		}
	}





	public class UrlHttpParameter : HttpParameter
	{
		public UrlHttpParameter (string key, object value) : base (key, value, ParameterType.Url)
		{
			if (!Types.IsNativeType (value))
				throw new ArgumentException ("Only native types are allowed for this kind of parameter");
		}
	}





	public class HeaderHttpParameter : HttpParameter
	{
		public HeaderHttpParameter (string key, string value) : base (key, value, ParameterType.Header)
		{

		}
	}

	public class ContentHttpParameter : HttpParameter
	{
		public ContentHttpParameter (string content) : base (null, content, ParameterType.Content)
		{

		}
	}

	public class JsonContentHttpParameter : ContentHttpParameter
	{
		public JsonContentHttpParameter (object content) : base (JsonConvert.SerializeObject (content))
		{

		}
	}
}

