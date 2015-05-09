using System.Linq;
using System;

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

	public class AuthorizationRules : Rules<AuthorizationRule, AuthorizationRulePredicate>
	{
		internal void Validate ()
		{
			if (this.Any (rule => rule.Method == null))
				throw new NotSupportedException ("All rules must specify an HttpMethod");
				
		}
	}

	public delegate bool AuthorizationRulePredicate (string path);

	public class AuthorizationRule : Rule<AuthorizationRulePredicate>
	{
		internal HttpMethod? Method { get; private set; }

		internal MinimumRole RoleRestriction { get; private set; }

		public MinimumRole WithMethod (HttpMethod method)
		{
			Method = method;
			RoleRestriction = new MinimumRole ();
			return RoleRestriction;
		}
	}

	public class MinimumRole
	{
		public int AtLeastRole { get; set; }
	}
}

