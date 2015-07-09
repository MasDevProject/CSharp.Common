using System.Web.Http.Filters;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http.Formatting;
using System.Linq;
using System.Net;
using System.Net.Http;
using MasDev.Common;

namespace MasDev.Services.Owin.WebApi
{
	public class ExceptionFilter : IExceptionFilter
	{
		public static event Action<Exception> OnUnhandledException;

		MediaTypeFormatter _formatter;

		public async Task ExecuteExceptionFilterAsync (HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
		{
			if (_formatter == null) {
				_formatter = actionExecutedContext.ActionContext.ControllerContext.Configuration.Formatters.First ();
			}

			var e = actionExecutedContext.Exception;

			var serviceException = e as ServiceException;
			if (serviceException == null && OnUnhandledException != null)
				OnUnhandledException (e);

			var statusCode = HttpStatusCode.InternalServerError;
			if (e is NotFoundException)
				statusCode = HttpStatusCode.NotFound;
			else if (e is InputException)
				statusCode = HttpStatusCode.BadRequest;
			else if (e is ForbiddenException)
				statusCode = HttpStatusCode.Forbidden;
			else if (e is UnauthorizedException)
				statusCode = HttpStatusCode.Unauthorized;

			var exContent = serviceException != null ? 
				serviceException.Content : 
				new ServiceExceptionContent (null, e.ToString ());

			var response = new HttpResponseMessage ();
			response.Content = new ObjectContent (typeof(ServiceExceptionContent), exContent, _formatter);
			response.StatusCode = statusCode;

			actionExecutedContext.Response = response;
		}

		public bool AllowMultiple { get { return true; } }
	}
}

