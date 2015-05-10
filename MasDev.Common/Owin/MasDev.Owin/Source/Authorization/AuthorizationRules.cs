using System.Linq;
using System;
using Microsoft.Owin;

namespace MasDev.Owin.Auth
{
	public enum HttpMethod
	{
		Get,
		Put,
		Post,
		Delete,
		Head,
		Trace,
		Connect,
		Options
	}

	static class HttpMethodNames
	{
		public const string Get = "get";
		public const string Put = "put";
		public const string Post = "post";
		public const string Delete = "delete";
		public const string Head = "head";
		public const string Trace = "trace";
		public const string Connect = "connect";
		public const string Options = "options";
	}

	public class AuthorizationRules : OwinMiddlewareRules<AuthorizationRule>
	{
		const string _cacheKeyFormat = "{0}::{1}";

		public override void Validate ()
		{
			if (this.Any (rule => rule.Method == null))
				throw new NotSupportedException ("All rules must specify an HttpMethod");
		}

		protected override AuthorizationRule FindMatchInternal (IOwinContext context)
		{
			var requestPath = context.Request.Path.ToString ();
			var method = ParseHttpMethod (context.Request.Method).Value;
			return this.FirstOrDefault (r => r.Method == method && r.Predicate (requestPath));
		}

		protected override string GetCacheKey (IOwinContext context)
		{
			return string.Format (_cacheKeyFormat, context.Request.Method, context.Request.Path);
		}

		public static HttpMethod? ParseHttpMethod (string method)
		{
			if (string.IsNullOrWhiteSpace (method))
				return null;

			switch (method.ToLowerInvariant ()) {
			case HttpMethodNames.Get:
				return HttpMethod.Get;
			case HttpMethodNames.Post:
				return HttpMethod.Post;
			case HttpMethodNames.Put:
				return HttpMethod.Put;
			case HttpMethodNames.Head:
				return HttpMethod.Head;
			case HttpMethodNames.Delete:
				return HttpMethod.Delete;
			case HttpMethodNames.Trace:
				return HttpMethod.Trace;
			case HttpMethodNames.Options:
				return HttpMethod.Options;
			default:
				return null;
			}
		}
	}

	public class AuthorizationRule : OwinMiddlewareRule
	{
		internal HttpMethod? Method { get; private set; }

		internal int? MinimumRoles { get; private set; }

		public AuthorizationRule WithMethod (HttpMethod method)
		{
			Method = method;
			return this;
		}

		public AuthorizationRule RequireAtLeast (int minimumRoles)
		{
			MinimumRoles = minimumRoles;
			return this;
		}
	}
}

