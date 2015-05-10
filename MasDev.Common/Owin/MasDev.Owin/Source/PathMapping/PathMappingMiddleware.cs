using Microsoft.Owin;
using MasDev.Owin.PathMapping;


namespace MasDev.Owin.Middlewares
{
	public abstract class PathMappingMiddleware : RuledMiddleware<PathMappingRules, PathMappingRule>
	{
		protected PathMappingMiddleware (PathMappingRules rules, OwinMiddleware next) : base (rules, next)
		{
		}
	}
}

