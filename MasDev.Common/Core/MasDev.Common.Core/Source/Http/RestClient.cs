using System;
using System.Net.Http;
using ModernHttpClient;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using MasDev.Extensions;
using MasDev.Utils;
using System.Text;
using System.Threading;
using MasDev.IO;
using System.IO;
using MasDev.Patterns.Injection;

namespace MasDev.Common.Http
{
	/*public class RestClient 
	{
		static HttpClient _client; // HttpClient is reusable and thread safe by design
		static IRestClientConfigurator _configurator;
		static RestClient _instance;

		public static RestClient Init (IRestClientConfigurator configurator)
		{
			if (_instance == null)
				_instance = new RestClient ();

			RestClient._configurator = configurator;
			return _instance;
		}

		static HttpClient GetClient ()
		{
			if (_configurator == null)
				throw new Exception ("Call RestClient.Init() first!");

			if (_client == null) {
				_client = _configurator.UseModerHttpClient ? new HttpClient (new NativeMessageHandler ()) : new HttpClient ();
				_configurator.AddHeaders (_client.DefaultRequestHeaders);
				_client.BaseAddress = new Uri(_configurator.BaseUrl);
				_client.Timeout = TimeSpan.FromMilliseconds (_configurator.RequestTimeoutInMillis);
			}

			return _client;
		}

		public static async Task<HttpResponse<T>> PostAsync<T> (bool useAuth, string relativePath, ICollection<HttpParameter> parameters, CancellationToken ct = default(CancellationToken))
		{
			return await BuildRequest<T> (useAuth, relativePath, HttpMethod.Post, parameters, ct);
		}

		public static async Task PostAsync (bool useAuth, string relativePath, ICollection<HttpParameter> parameters, CancellationToken ct = default(CancellationToken))
		{
			await BuildRequest<string> (useAuth, relativePath, HttpMethod.Post, parameters, ct);
		}

		public static async Task<HttpResponse<T>> PutAsync<T> (bool useAuth, string relativePath, ICollection<HttpParameter> parameters, CancellationToken ct = default(CancellationToken))
		{
			return await BuildRequest<T> (useAuth, relativePath, HttpMethod.Put, parameters, ct);
		}

		public static async Task PutAsyn (bool useAuth, string relativePath, ICollection<HttpParameter> parameters, CancellationToken ct = default(CancellationToken))
		{
			await BuildRequest<string> (useAuth, relativePath, HttpMethod.Put, parameters, ct);
		}

		public static async Task<HttpResponse<T>> GetAsync<T> (bool useAuth, string relativePath, ICollection<HttpParameter> parameters, CancellationToken ct = default(CancellationToken))
		{
			return await BuildRequest<T> (useAuth, relativePath, HttpMethod.Get, parameters, ct);
		}

		public static async Task<HttpResponse<T>> DeleteAsync<T> (bool useAuth, string relativePath, ICollection<HttpParameter> parameters, CancellationToken ct = default(CancellationToken))
		{
			return await BuildRequest<T> (useAuth, relativePath, HttpMethod.Delete, parameters, ct);
		}

		static async Task<HttpResponse<T>> BuildRequest<T> (bool useAuth, string relativeUrl, HttpMethod method, ICollection<HttpParameter> parameters, CancellationToken ct = default(CancellationToken))
		{
			ct.ThrowIfCancellationRequested ();

			var urlBuilder = new StringBuilder (GetClient ().BaseAddress.ToString ());
			urlBuilder.Append (relativeUrl);
			parameters = parameters ?? new List<HttpParameter> ();
			var request = new HttpRequestMessage ();
			request.Method = method;
			var multiPartContent = parameters.SingleOrDefault (p => p.ParameterType == ParameterType.FormMultipart) as FormMultipartHttpParameter;

			#region Auth
			if (useAuth) {
				var authInfo = _configurator.Credentials;
				request.Headers.Authorization = new AuthenticationHeaderValue (authInfo.Key, authInfo.Value);
			}
			#endregion

			#region Headers
			parameters.Where (p => p.ParameterType == ParameterType.Header).ForEach(h => request.Headers.TryAddWithoutValidation (h.First, h.Second));
			#endregion

			#region Form
			if (multiPartContent == null) {
				var formParam = parameters.SingleOrDefault (p => p.ParameterType == ParameterType.Form);
				if (formParam != null)
					request.Content = new StringContent (formParam.Second, Encoding.UTF8);
			}
			#endregion

			#region Form url encoded
			if (multiPartContent == null) {
				var formParams = parameters.Where (p => p.ParameterType == ParameterType.FormUrlEncoded);
				var formContent = new FormUrlEncodedContent(new KeyValuePair<string, string>[] formParams);
				request.Content = formContent;
			}
			#endregion

			#region Url parameters
			var urlParameters = parameters.Where (p => p.ParameterType == ParameterType.Url);
			foreach(var p in urlParameters)
			{
				var paramWildcard = "{" + p.Key + "}";
				urlBuilder.Replace (paramWildcard, p.Value);
			}
			#endregion

			#region Query
			var queryParameters = parameters.Where (p => p.ParameterType == ParameterType.Query).ToArray ();
			for (var i = 0; i < queryParameters.Length; i++) {
				if (i == 0)
					urlBuilder.Append ('?');

				var queryParameter = queryParameters [i];
				urlBuilder.Append (queryParameter.Key);
				urlBuilder.Append ('=');
				urlBuilder.Append (WebUtility.UrlEncode (queryParameter.Value));

				if (i != queryParameters.Length - 1)
					urlBuilder.Append ('&');
			}
			#endregion

			request.RequestUri = urlBuilder.ToString ().AsUri ();

			var logger = Injector.Resolve<ILogger> ();
			if (logger != null)
				logger.Log ("Generated URL: " + request.RequestUri);

			#region Send

			ct.ThrowIfCancellationRequested ();
			HttpResponseMessage response;
			if (multiPartContent != null) 
			{
				var ms = multiPartContent.MimedStream;
				using (var content = new MultipartFormDataContent ())
				using (var imageContent = new StreamContent (ms.Stream)) {
					content.Add (imageContent, multiPartContent.Key, ms.FileName.Replace (StringUtils.Space, string.Empty));
					imageContent.Headers.ContentType = new MediaTypeHeaderValue (ms.Mime);
					request.Content = content;

					response = await GetClient ().SendAsync (request);
					return await AfterExecution<T> (response, ct);
				}	
			} 
			else 
			{
				response = await GetClient ().SendAsync (request);
			}

			#endregion

			return typeof(T) == typeof(Stream) ? await AfterExecutionAsStream<T> (response, ct) : await AfterExecution<T> (response, ct);
		}

		static async Task<HttpResponse<T>> AfterExecution<T> (HttpResponseMessage response, CancellationToken ct)
		{
			ct.ThrowIfCancellationRequested ();
			using (var content = response.Content) {
				var responseString = await content.ReadAsStringAsync ();
				if (response.StatusCode != HttpStatusCode.OK)
					_configurator.HandleNonOkResponse (response.StatusCode, responseString);

				return new HttpResponse<T> (_configurator.Deserialize<T> (responseString), response);
			}
		}

		static async Task<HttpResponse<T>> AfterExecutionAsStream<T> (HttpResponseMessage response, CancellationToken ct)
		{
			ct.ThrowIfCancellationRequested ();
			using (var content = response.Content) {

				if (response.StatusCode != HttpStatusCode.OK)
					_configurator.HandleNonOkResponse (response.StatusCode, string.Empty);

				return new HttpResponse<T> (await content.ReadAsStreamAsync (), response);
			}
		}
	}

	public interface IRestClientConfigurator
	{
		void AddHeaders (HttpRequestHeaders header);

		bool UseModerHttpClient { get; }

		string BaseUrl { get; }

		KeyValuePair<string, string> Credentials { get; }

		void HandleNonOkResponse (HttpStatusCode statusCode, string content);

		T Deserialize<T> (string responseString);

		string Serialized (object @object);

		int RequestTimeoutInMillis { get; }
	}

	#region Params

	public class HttpResponse<T>
	{
		public T Content { get; private set; }
		public HttpResponseMessage Response { get; private set; }

		public HttpResponse(T deserializedObject, HttpResponseMessage response)
		{
			Content = deserializedObject;
			Response = response;
		}
	}

	public enum ParameterType
	{
		Query,
		Url,
		Form,
		FormMultipart,
		FormUrlEncoded,
		Header
	}

	public class HttpParameter : Pair<string, string>
	{
		public ParameterType ParameterType { get; private set; }

		public HttpParameter (string key, string value, ParameterType paramType) : base (key, value)
		{
			ParameterType = paramType;
		}
	}

	public class FormHttpParameter : HttpParameter
	{
		public FormHttpParameter (string content) : base (null, content, ParameterType.Form) //ignore key
		{
		}
	}

	public class FormUrlEncodedParameter : HttpParameter
	{
		public FormUrlEncodedParameter (string key, string value) : base (key, value, ParameterType.FormUrlEncoded)
		{
		}
	}

	public class FormMultipartHttpParameter : HttpParameter
	{
		public MimedStream MimedStream {get; private set; }
		public FormMultipartHttpParameter (string key, MimedStream content) : base (key, null, ParameterType.FormMultipart) //ignore value
		{
			MimedStream = content;
		}
	}

	public class QueryHttpParameter : HttpParameter
	{
		public QueryHttpParameter (string key, string value) : base (key, value, ParameterType.Query)
		{
		}
	}

	public class UrlHttpParameter : HttpParameter
	{
		public UrlHttpParameter (string key, string value) : base (key, value, ParameterType.Url)
		{
		}
	}

	public class HeaderHttpParameter : HttpParameter
	{
		public HeaderHttpParameter (string key, string value) : base (key, value, ParameterType.Header)
		{
		}
	}

	#endregion
	*/
}