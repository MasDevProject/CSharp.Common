using System;
using Microsoft.Owin;
using System.Threading.Tasks;
using System.Linq;

namespace MasDev.Owin.Middlewares
{
	public class RedirectMiddleware : BasePathMappingMiddleware
	{
		public RedirectMiddleware (OwinMiddleware next) : base (next, typeof(RedirectMiddleware).Name)
		{
		}

		public override async Task Invoke (IOwinContext context)
		{
			var requestPath = context.Request.Path.ToString ();
			var matchingRule = MappingRules.Rules.FirstOrDefault (rule => rule.Predicate (requestPath));
			if (matchingRule == null) {
				await Next.Invoke (context);
				return;
			}

			context.Response.Redirect (requestPath);
		}
	}
}

