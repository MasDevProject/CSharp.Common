using System.Web.Http.Filters;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Net.Http;
using System.Linq;
using System.Net.Http.Formatting;
using System;
using System.Web.Http.Controllers;
using Newtonsoft.Json;
using MasDev.Common.Rest.Auth;


namespace MasDev.Common.Rest.WebApi
{
	public class ExceptionLoggerFilter : IExceptionFilter
	{
		volatile MediaTypeFormatter _formatter;
		JsonSerializerSettings _settings;
		const string _header = "==============================================================";



		public Task ExecuteExceptionFilterAsync (HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
		{
			if (_formatter == null)
			{
				_formatter = actionExecutedContext.ActionContext.ControllerContext.Configuration.Formatters.First ();
				_settings = actionExecutedContext.ActionContext.ControllerContext.Configuration.Formatters.JsonFormatter.SerializerSettings;
			}

			WriteLoggingHeader (actionExecutedContext.ActionContext);

			var e = actionExecutedContext.Exception;

			var baseEx = e as BaseApiException;
			var exContent = baseEx != null ? 
				baseEx.Content : 
				new ApiExceptionContent (HttpStatusCode.InternalServerError, null, e.ToString ());

			Debug.WriteLine ("Exception: " + exContent.StatusCode);
			Debug.WriteLine ("AdditionalInfo: " + exContent.AdditionalInformation);
			Debug.WriteLine ("Content:\n\t" + JsonConvert.SerializeObject (exContent.Content, _settings));

			var response = new HttpResponseMessage ();
			response.Content = new ObjectContent (typeof(ApiExceptionContent), exContent, _formatter);
			response.StatusCode = exContent.StatusCode;

			actionExecutedContext.Response = response;
			return Task.Delay (0);
		}



		public bool AllowMultiple { get { return true; } }



		public static void WriteLoggingHeader (HttpActionContext actionContext)
		{
			Debug.WriteLine ("\n" + _header);
			Debug.WriteLine ("[" + DateTime.Now.ToShortTimeString () + "] " + actionContext.Request.RequestUri);
			Debug.WriteLine ("Arguments:");

			var actionArguments = actionContext.ActionArguments;
			if (actionArguments == null || !actionArguments.Any ())
				Debug.WriteLine ("\tnone");
			else
			{
				foreach (var arg in actionArguments)
					Debug.WriteLine ("\t" + arg.Key + ": " + arg.Value);
			}
		}
	}
}

