using System;

namespace MasDev.Owin.ErrorPage
{
	public class ErrorPageRules : Rules<ErrorPageRule, ErrorPageRulePredicate>
	{
	}

	public delegate bool ErrorPageRulePredicate (int statusCode, string requestPath);

	public class ErrorPageRule : Rule<ErrorPageRulePredicate>
	{
		string _errorPagePath;

		public string ErrorPagePath { 
			get { return _errorPagePath; }
			set {
				if (string.IsNullOrEmpty (value))
					throw new ArgumentNullException ();
				_errorPagePath = value;
			}
		}
	}
}