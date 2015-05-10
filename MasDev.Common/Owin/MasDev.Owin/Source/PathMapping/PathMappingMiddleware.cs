using Microsoft.Owin;
using MasDev.Owin.PathMapping;


namespace MasDev.Owin.Middlewares
{
	public abstract class PathMappingMiddleware : RuledMiddleware<PathMappingRules, PathMappingRule>
	{
		protected PathMappingMiddleware (OwinMiddleware next, PathMappingRules rules) : base (next, rules)
		{
		}
	}
}

