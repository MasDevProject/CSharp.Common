using System.Linq;
using System;
using Microsoft.Owin;


namespace MasDev.Services.Middlewares
{
	public class BasePathMappingRules : OwinMiddlewareRules<PathMappingRule>
	{
		public override void Validate ()
		{
			foreach (var rule in this) {
				var mapPath = rule.MapPath;

				if (string.IsNullOrWhiteSpace (mapPath))
					throw new NotSupportedException ("MapPath must not be null");

				if (this.Any (r => r.Predicate (mapPath)))
					throw new PathMappingException ("Path mapping destination '{0}' maps to other math mapping rule", mapPath);
			}
		}

		protected override PathMappingRule FindMatchInternal (IOwinContext context)
		{
			var requestPath = context.Request.Path.NormalizePath ();
			return this.FirstOrDefault (r => r.Predicate (requestPath));
		}

		protected override string GetCacheKey (IOwinContext context)
		{
			return context.Request.Path.ToString ();
		}
	}

	public sealed class PathMappingRule : OwinMiddlewareRule
	{
		internal string MapPath { get; set; }

		public PathMappingRule MapTo (string path)
		{
			MapPath = path;
			return this;
		}
	}
}

