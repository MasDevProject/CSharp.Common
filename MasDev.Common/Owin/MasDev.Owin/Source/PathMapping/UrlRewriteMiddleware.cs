using System;
using Microsoft.Owin;
using System.Threading.Tasks;
using System.Linq;
using MasDev.Owin.Middlewares;

namespace MasDev.Owin.UrlRewrite
{
	public class UrlRewriteMiddleware : BasePathMappingMiddleware
	{
		public UrlRewriteMiddleware (OwinMiddleware next) : base (next, typeof(UrlRewriteMiddleware).Name)
		{
		}

		public override async Task Invoke (IOwinContext context)
		{
			var requestPath = context.Request.Path.ToString ();
			var matchingRule = MappingRules.FirstOrDefault (rule => rule.Predicate (requestPath));
			if (matchingRule != null)
				context.Request.Path = new PathString (matchingRule.MapTo);

			await Next.Invoke (context);
		}
	}
}

