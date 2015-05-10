using System.Collections.Generic;
using Microsoft.Owin;


namespace MasDev.Owin
{
	public abstract class OwinMiddlewareRules<TRule> : IEnumerable<TRule> where TRule : OwinMiddlewareRule, new()
	{
		readonly IList<TRule> _rules = new List<TRule> ();

		public IEnumerator<TRule> GetEnumerator ()
		{
			return _rules.GetEnumerator ();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return _rules.GetEnumerator ();
		}

		public virtual void Validate ()
		{

		}

		public abstract TRule FindMatch (IOwinContext context);

		public void When (OwinMiddlewareRulePredicate predicate)
		{
			predicate.ThrowIfNull ("predicate");
			var rule = new TRule ();
			rule.Predicate = predicate;
			_rules.Add (rule);
		}
	}

	public delegate bool OwinMiddlewareRulePredicate (string path);
		

	public class OwinMiddlewareRule
	{
		internal OwinMiddlewareRulePredicate Predicate { get; set; }

		public bool IsCacheEnabled { get; private set; }

		internal OwinMiddlewareRule ()
		{
			// No public constructor
			IsCacheEnabled = true;
		}

		public void UseCache (bool useCache)
		{
			IsCacheEnabled = useCache;
		}
	}
}

