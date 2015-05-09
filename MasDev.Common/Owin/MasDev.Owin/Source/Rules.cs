using System.Collections.Generic;


namespace MasDev.Owin
{
	public class Rules<TRule, TPredicate> : IEnumerable<TRule> where TRule : Rule<TPredicate>, new()
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

		public void When (TPredicate predicate)
		{
			predicate.ThrowIfNull ("predicate");
			var rule = new TRule ();
			rule.Predicate = predicate;
			_rules.Add (rule);
		}
	}

	public class Rule<TPredicate>
	{
		public TPredicate Predicate { get; internal set; }

		internal Rule ()
		{
			// No public constructor
		}
	}
}

