using Microsoft.Owin;
using System.Threading.Tasks;
using MasDev.Patterns.Injection;
using MasDev.Common;

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

			var language = _fallbackLanguage;
			var headers = context.Request.Headers;
			if (headers.ContainsKey (AcceptLanguageHeaderName)) {
				var languages = headers ["Accept-Language"];
				if (languages != null)
					language = languages.Split (',') [0].Trim ().ToLowerInvariant ();
				if (string.IsNullOrWhiteSpace (language))
					language = _fallbackLanguage;
			}

			callingContext.Language = language;
			await Next.Invoke (context);
		}
	}
}

