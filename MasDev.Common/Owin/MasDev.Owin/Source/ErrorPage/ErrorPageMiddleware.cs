using System;
using Microsoft.Owin;
using System.Threading.Tasks;
using MasDev.Owin.ErrorPage;
using System.Linq;

namespace MasDev.Owin.Middlewares
{
	public class ErrorPageMiddleware : OwinMiddleware
	{
		public static ErrorPageRules Rules { get; set; }

		public ErrorPageMiddleware (OwinMiddleware next) : base (next)
		{
			if (Rules == null)
				throw new NotSupportedException (string.Format ("Set error page rules before using {0}", typeof(ErrorPageMiddleware).Name));
		}

		public override async Task Invoke (IOwinContext context)
		{
			var requestPath = context.Request.Path.ToString ();
			await Next.Invoke (context);
			var statusCode = context.Response.StatusCode;

			if (statusCode == 200)
				return;

			var matchingRule = Rules.FirstOrDefault (rule => rule.Predicate (statusCode, requestPath));
			if (matchingRule != null)
				context.Response.Redirect (matchingRule.ErrorPagePath);
		}

	}
}

