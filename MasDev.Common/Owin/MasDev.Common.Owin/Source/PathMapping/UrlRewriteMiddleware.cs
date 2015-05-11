using Microsoft.Owin;
using System.Threading.Tasks;
using MasDev.Common.Owin.Middlewares;
using MasDev.Common.Owin.Rules;

namespace MasDev.Common.Owin.Middlewares
{
	public class UrlRewriteMiddleware : PathMappingMiddleware
	{
		public UrlRewriteMiddleware (OwinMiddleware next, PathMappingRules rules) : base (next, rules)
		{
		}

		public override async Task Invoke (IOwinContext context)
		{
			var matchingRule = Rules.FindMatch (context);
			if (matchingRule != null)
				context.Request.Path = new PathString (matchingRule.MapPath);

			await Next.Invoke (context);
		}
	}
}

