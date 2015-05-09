using System.Linq;
using System;


namespace MasDev.Owin.PathMapping
{

	public class PathMappingRules : Rules<PathMappingRule, PathMappingRulePredicate>
	{
		internal void Validate ()
		{
			foreach (var rewrite in this) {
				var rewriteTo = rewrite.MapTo;

				if (this.Any (r => r.Predicate (rewriteTo)))
					throw new PathMappingException ("Path mapping destination '{0}' maps to other math mapping rule", rewriteTo);
			}
		}
	}

	public delegate bool PathMappingRulePredicate (string requestPath);


	public class PathMappingRule : Rule<PathMappingRulePredicate>
	{
		string _mapTo;

		public string MapTo { 
			get { return _mapTo; } 
			set {
				if (string.IsNullOrWhiteSpace (value))
					throw new ArgumentNullException ();
				_mapTo = value;
			}
		}
	}
}

