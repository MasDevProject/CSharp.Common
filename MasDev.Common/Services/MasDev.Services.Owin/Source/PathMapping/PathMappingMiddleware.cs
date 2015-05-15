using Microsoft.Owin;


namespace MasDev.Services.Middlewares
{
	public abstract class PathMappingMiddleware : RuledMiddleware<PathMappingRules, PathMappingRule>
	{
		protected PathMappingMiddleware (OwinMiddleware next, PathMappingRules rules) : base (next, rules)
		{
		}
	}
}

