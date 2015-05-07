using System.Collections.Generic;
using System.Linq;
using System;


namespace MasDev.Owin.PathMapping
{

	public class PathMappingRules : IEnumerable<PathMappingRule>
	{

		readonly IList<PathMappingRule> _rules = new List<PathMappingRule> ();


		public PathMappingRule When (PathMappingPredicate predicate)
		{
			var rewrite = new PathMappingRule (predicate);
			_rules.Add (rewrite);
			return rewrite;
		}

		internal void Validate ()
		{
			foreach (var rewrite in this) {
				var rewriteTo = rewrite.MapTo;

				if (this.Any (r => r.Predicate (rewriteTo)))
					throw new PathMappingException ("Path mapping destination '{0}' maps to other math mapping rule", rewriteTo);
			}
		}

		public IEnumerator<PathMappingRule> GetEnumerator ()
		{
			return _rules.GetEnumerator ();
		}


		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return _rules.GetEnumerator ();
		}

	}

	public delegate bool PathMappingPredicate (string requestPath);


	public class PathMappingRule
	{
		internal PathMappingPredicate Predicate { get; set; }

		string _mapTo;

		public string MapTo { 
			get { return _mapTo; } 
			set {
				if (string.IsNullOrWhiteSpace (value))
					throw new ArgumentNullException ();
				_mapTo = value;
			}
		}

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

