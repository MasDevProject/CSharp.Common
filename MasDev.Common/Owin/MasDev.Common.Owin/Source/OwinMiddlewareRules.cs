using System.Collections.Generic;
using Microsoft.Owin;
using System.Collections.Concurrent;
using System;


namespace MasDev.Common.Owin.Rules
{
	public abstract class OwinMiddlewareRules<TRule> : IEnumerable<TRule> where TRule : OwinMiddlewareRule, new()
	{
		readonly ConcurrentDictionary<string, TRule> _cache = new ConcurrentDictionary<string, TRule> ();
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

		public TRule FindMatch (IOwinContext context)
		{
			TRule rule;
			var cacheKey = GetCacheKey (context);
			if (_cache.ContainsKey (cacheKey) && _cache.TryGetValue (cacheKey, out rule))
				return rule;

			rule = FindMatchInternal (context);
			if (rule == null || rule.IsCacheEnabled)
				_cache.AddOrUpdate (cacheKey, rule, (key, old) => rule);

			return rule;
		}

		protected abstract TRule FindMatchInternal (IOwinContext context);

		protected abstract string GetCacheKey (IOwinContext context);

		public TRule When (OwinMiddlewareRulePredicate predicate)
		{
			predicate.ThrowIfNull ("predicate");
			var rule = new TRule ();
			rule.Predicate = predicate;
			_rules.Add (rule);
			return rule;
		}

		public TRule WhenMatches (string templatePath)
		{
			return When (path => path.MatchesTemplate (templatePath));
		}

		public TRule WhenEquals (string url)
		{
			return When (path => path == url);
		}

		public TRule WhenStartsWith (string url, StringComparison? comparison = null)
		{
			return When (path => path.StartsWith (url, comparison ?? StringComparison.OrdinalIgnoreCase));
		}

		public TRule WhenContains (string what)
		{
			return When (path => path.Contains (what));
		}
	}

	public delegate bool OwinMiddlewareRulePredicate (string path);
		

	public class OwinMiddlewareRule
	{
		internal OwinMiddlewareRulePredicate Predicate { get; set; }

		internal bool IsCacheEnabled { get; private set; }

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

