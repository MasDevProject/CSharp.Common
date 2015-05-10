using System.Linq;
using System;
using Microsoft.Owin;


namespace MasDev.Owin.PathMapping
{
	public class PathMappingRules : OwinMiddlewareRules<PathMappingRule>
	{
		public override void Validate ()
		{
			foreach (var rule in this) {
				var mapPath = rule.MapPath;

				if (string.IsNullOrWhiteSpace (mapPath))
					throw new ArgumentNullException ("mapPath");

				if (this.Any (r => r.Predicate (mapPath)))
					throw new PathMappingException ("Path mapping destination '{0}' maps to other math mapping rule", mapPath);
			}
		}


		public override PathMappingRule FindMatch (IOwinContext context)
		{
			var requestPath = context.Request.Path.ToString ();
			// TODO cache
			return this.FirstOrDefault (r => r.Predicate (requestPath));
		}
	}

	public class PathMappingRule : OwinMiddlewareRule
	{
		internal string MapPath { get; set; }

		public PathMappingRule MapTo (string path)
		{
			MapPath = path;
			return this;
		}
	}
}

