using Microsoft.Owin;
using System.Threading.Tasks;
using MasDev.Patterns.Injection;
using MasDev.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MasDev.Services
{
	public class CallingContextMiddleware : OwinMiddleware
	{
		const string AcceptLanguageHeaderName = "Accept-Language";
		readonly string _fallbackLanguage;

		public CallingContextMiddleware (OwinMiddleware next, string fallbackLanguage) : base (next)
		{
			_fallbackLanguage = fallbackLanguage;
		}


		public override async Task Invoke (IOwinContext context)
		{
			var callingContext = Injector.Resolve<ICallingContext> ();

			callingContext.RequestPath = context.Request.Uri.AbsoluteUri;
			callingContext.RequestIp = context.Request.RemoteIpAddress;
			callingContext.RequestHost = context.Request.Host.Value;
			callingContext.RequestHeaders = new MultiValueDictionary<string, string> ();

			foreach (var key in context.Request.Headers.Keys)
				callingContext.RequestHeaders.AddValues (context.Request.Headers [key]);

			var language = _fallbackLanguage;
			var headers = context.Request.Headers;
			if (headers.ContainsKey (AcceptLanguageHeaderName)) {
				var languages = headers [AcceptLanguageHeaderName];
				if (languages != null)
					language = languages.Split (',') [0].Trim ().ToLowerInvariant ();
				if (string.IsNullOrWhiteSpace (language))
					language = _fallbackLanguage;
			}

			callingContext.Language = language;
			await Next.Invoke (context);

			var responseHeders = callingContext.ResponseHeaders;
			if (responseHeders == null || !responseHeders.Any ())
				return;

			var owinResponsHeaders = context.Response.Headers;
			foreach (var key in responseHeders.Keys) {
				if (!owinResponsHeaders.ContainsKey (key))
					owinResponsHeaders.Add (key, new string[0]);

				owinResponsHeaders.AppendCommaSeparatedValues (key, responseHeders [key].ToArray ());
			}
		}
	}
}

