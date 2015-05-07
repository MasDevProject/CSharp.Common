using System.Collections.Generic;
using System.Linq;
using System;


namespace MasDev.Owin.PathMapping
{
	public delegate bool PathMappingPredicate (string requestPath);

	public class PathMappingRules
	{
		internal IEnumerable<PathMappingRule> Rules { get { return _rule.AsEnumerable (); } }

		readonly IList<PathMappingRule> _rule = new List<PathMappingRule> ();


		public PathMappingRule When (PathMappingPredicate predicate)
		{
			var rewrite = new PathMappingRule (predicate);
			_rule.Add (rewrite);
			return rewrite;
		}

		internal void Validate ()
		{
			foreach (var rewrite in Rules) {
				var rewriteTo = rewrite.MapTo;
				if (string.IsNullOrWhiteSpace (rewriteTo))
					throw new PathMappingException ("Path mapping destination cannot be null");

				if (Rules.Any (r => r.Predicate (rewriteTo)))
					throw new PathMappingException ("Path mapping destination '{0}' maps to other math mapping rule", rewriteTo);
			}
		}
	}


	public class PathMappingRule
	{
		internal PathMappingPredicate Predicate { get; set; }

		public string MapTo { get; set; }

		PathMappingRule ()
		{
			// No public constructor
		}

		internal PathMappingRule (PathMappingPredicate predicate)
		{
			Predicate = predicate;
		}
	}
}

