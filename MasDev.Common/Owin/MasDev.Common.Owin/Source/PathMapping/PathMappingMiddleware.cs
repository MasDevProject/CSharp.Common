using Microsoft.Owin;
using MasDev.Common.Owin.Rules;


namespace MasDev.Common.Owin.Middlewares
{
	public abstract class PathMappingMiddleware : RuledMiddleware<PathMappingRules, PathMappingRule>
	{
		protected PathMappingMiddleware (OwinMiddleware next, PathMappingRules rules) : base (next, rules)
		{
		}
	}
}

