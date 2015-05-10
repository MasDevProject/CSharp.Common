using Microsoft.Owin;
using System.Threading.Tasks;
using MasDev.Owin.Middlewares;
using MasDev.Owin.PathMapping;

namespace MasDev.Owin.Middlewares
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

