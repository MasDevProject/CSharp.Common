using System;
using System.Linq;
using Microsoft.Owin;

namespace MasDev.Owin.ErrorPage
{
	public class ErrorPageRules : OwinMiddlewareRules<ErrorPageRule>
	{
		const string _cacheKeyFormat = "{0}::{1}";

		public override void Validate ()
		{
			foreach (var rule in this) {
				if (rule.StatusCodes == null)
					throw new NotSupportedException ("StatusCodes are mandatory");
				if (string.IsNullOrWhiteSpace (rule.ErrorPagePath))
					throw new NotSupportedException ("ErrorPagePath is mandatory");
			}
		}

		protected override ErrorPageRule FindMatchInternal (IOwinContext context)
		{
			var requestPath = context.Request.Path.ToString ();
			var statusCode = context.Response.StatusCode;

			return this.FirstOrDefault (rule => rule.StatusCodes.Any (c => c == statusCode) && rule.Predicate (requestPath));
		}

		protected override string GetCacheKey (IOwinContext context)
		{
			return string.Format (_cacheKeyFormat, context.Response.StatusCode, context.Request.Path);
		}
	}

	public class ErrorPageRule : OwinMiddlewareRule
	{
		internal int[] StatusCodes { get; private set; }

		internal string ErrorPagePath { get; private set; }

		public ErrorPageRule ForStatusCodes (params int[] statusCodes)
		{
			StatusCodes = statusCodes;
			return this;
		}

		public ErrorPageRule ShowErrorPage (string errorPagePath)
		{
			ErrorPagePath = errorPagePath;
			return this;
		}
	}
}