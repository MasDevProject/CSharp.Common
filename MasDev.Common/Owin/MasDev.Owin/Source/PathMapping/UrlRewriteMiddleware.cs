using System;
using Microsoft.Owin;
using System.Threading.Tasks;
using System.Linq;
using MasDev.Owin.Middlewares;
using MasDev.Owin.PathMapping;

namespace MasDev.Owin.Middlewares
{
	public class UrlRewriteMiddleware : BasePathMappingMiddleware
	{
		public UrlRewriteMiddleware (PathMappingRules rules, OwinMiddleware next) : base (rules, next)
		{
		}

		public override async Task Invoke (IOwinContext context)
		{
			var requestPath = context.Request.Path.ToString ();
			var matchingRule = Rules.FirstOrDefault (rule => rule.Predicate (requestPath));
			if (matchingRule != null)
				context.Request.Path = new PathString (matchingRule.MapTo);

			await Next.Invoke (context);
		}
	}
}

