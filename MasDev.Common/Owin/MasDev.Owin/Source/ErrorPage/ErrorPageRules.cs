using System;
using System.Linq;
using Microsoft.Owin;

namespace MasDev.Owin.ErrorPage
{
	public class ErrorPageRules : OwinMiddlewareRules<ErrorPageRule>
	{
		public override void Validate ()
		{
			foreach (var rule in this) {
				if (rule.StatusCodes == null)
					throw new NotSupportedException ("StatusCodes are mandatory");
				if (string.IsNullOrWhiteSpace (rule.ErrorPagePath))
					throw new NotSupportedException ("ErrorPagePath is mandatory");
			}
		}

		public override ErrorPageRule FindMatch (IOwinContext context)
		{
			var requestPath = context.Request.Path.ToString ();
			var statusCode = context.Response.StatusCode;

			// TODO cache
			return this.FirstOrDefault (rule => rule.StatusCodes.Any (c => c == statusCode) && rule.Predicate (requestPath));
		}
	}

	public class ErrorPageRule : OwinMiddlewareRule
	{
		internal int[] StatusCodes { get; private set; }

		internal string ErrorPagePath { get; private set; }

		public ErrorPageRule ForStatusCodes (int[] statusCodes)
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