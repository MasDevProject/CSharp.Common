using Microsoft.Owin;
using MasDev.Owin.PathMapping;


namespace MasDev.Owin.Middlewares
{
	public abstract class BasePathMappingMiddleware : RuledMiddleware<PathMappingRules, PathMappingRule, PathMappingRulePredicate>
	{
		protected BasePathMappingMiddleware (PathMappingRules rules, OwinMiddleware next) : base (rules, next)
		{
			Rules.Validate ();
		}
	}
}

