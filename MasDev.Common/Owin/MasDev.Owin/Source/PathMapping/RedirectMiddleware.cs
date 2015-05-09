using System;
using Microsoft.Owin;
using System.Threading.Tasks;
using System.Linq;
using MasDev.Owin.PathMapping;

namespace MasDev.Owin.Middlewares
{
	public class RedirectMiddleware : BasePathMappingMiddleware
	{
		public RedirectMiddleware (PathMappingRules rules, OwinMiddleware next) : base (rules, next)
		{
		}

		public override async Task Invoke (IOwinContext context)
		{
			var requestPath = context.Request.Path.ToString ();
			var matchingRule = Rules.FirstOrDefault (rule => rule.Predicate (requestPath));
			if (matchingRule != null) {
				await Next.Invoke (context);
				return;
			}

			context.Response.Redirect (matchingRule.MapTo);
		}
	}
}

