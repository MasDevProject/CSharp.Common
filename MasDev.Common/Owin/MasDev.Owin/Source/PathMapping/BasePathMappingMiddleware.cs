using Microsoft.Owin;
using MasDev.Owin.PathMapping;
using System;


namespace MasDev.Owin.Middlewares
{
	public class BasePathMappingMiddleware : OwinMiddleware
	{
		public static PathMappingRules MappingRules { get; set; }

		public BasePathMappingMiddleware (OwinMiddleware next, string middlewareName) : base (next)
		{
			if (MappingRules == null)
				throw new NotSupportedException (string.Format ("Set rewrite rules before using {0}", middlewareName));
			MappingRules.Validate ();
		}

	}
}

