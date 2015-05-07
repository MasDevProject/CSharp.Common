using System.Collections.Generic;
using System;

namespace MasDev.Owin.ErrorPage
{
	public class ErrorPageRules : IEnumerable<ErrorPageRule>
	{
		readonly IList<ErrorPageRule> _rules = new List<ErrorPageRule> ();


		public ErrorPageRule When (ErrorPageRulePredicate predicate)
		{
			var rule = new ErrorPageRule (predicate);
			_rules.Add (rule);
			return rule;
		}


		public IEnumerator<ErrorPageRule> GetEnumerator ()
		{
			return _rules.GetEnumerator ();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return _rules.GetEnumerator ();
		}

	}

	public delegate bool ErrorPageRulePredicate (int statusCode, string requestPath);

	public class ErrorPageRule
	{
		internal ErrorPageRulePredicate Predicate { get; set; }

		string _errorPagePath;

		public string ErrorPagePath { 
			get { return _errorPagePath; }
			set {
				if (string.IsNullOrEmpty (value))
					throw new ArgumentNullException ();
				_errorPagePath = value;
			}
		}

		ErrorPageRule ()
		{
			// no public constructors
		}

		internal ErrorPageRule (ErrorPageRulePredicate predicate)
		{
			Predicate = predicate;
		}
	}
}

