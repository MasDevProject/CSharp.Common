﻿using System;
using Microsoft.Owin;
using System.Threading.Tasks;
using MasDev.Owin.ErrorPage;

namespace MasDev.Owin.Middlewares
{
	public class ErrorPageMiddleware : RuledMiddleware<ErrorPageRules, ErrorPageRule>
	{
		public ErrorPageMiddleware (ErrorPageRules rules, OwinMiddleware next) : base (rules, next)
		{
		}

		public override async Task Invoke (IOwinContext context)
		{
			await Next.Invoke (context);
		
			var statusCode = context.Response.StatusCode;

			if (statusCode == 200)
				return;
			
			var matchingRule = Rules.FindMatch (context);
			if (matchingRule == null)
				return;
				
			await context.Response.SendFileAsync (matchingRule.ErrorPagePath);
			context.Response.StatusCode = 404;
		}
	}
}

