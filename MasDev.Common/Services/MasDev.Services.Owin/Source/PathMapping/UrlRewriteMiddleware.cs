﻿using Microsoft.Owin;
using System.Threading.Tasks;

namespace MasDev.Services.Middlewares
{
	public class UrlRewriteMiddleware : PathMappingMiddleware
	{
		public UrlRewriteMiddleware (OwinMiddleware next, BasePathMappingRules rules) : base (next, rules)
		{
		}

		public UrlRewriteMiddleware (OwinMiddleware next, string configFilePath) : base (next, configFilePath)
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

