using Microsoft.Owin;
using System.Threading.Tasks;

namespace MasDev.Services.Middlewares
{
	public class RedirectMiddleware : PathMappingMiddleware
	{
		public RedirectMiddleware (OwinMiddleware next, BasePathMappingRules rules) : base (next, rules)
		{
		}

		public override async Task Invoke (IOwinContext context)
		{
			var matchingRule = Rules.FindMatch (context);
			if (matchingRule != null) {
				context.Response.Redirect (matchingRule.MapPath);
				return;
			}
			await Next.Invoke (context);
		}
	}
}

