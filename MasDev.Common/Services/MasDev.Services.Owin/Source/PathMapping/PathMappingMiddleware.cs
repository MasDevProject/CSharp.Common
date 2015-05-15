using Microsoft.Owin;


namespace MasDev.Services.Middlewares
{
	public abstract class PathMappingMiddleware : RuledMiddleware<BasePathMappingRules, PathMappingRule>
	{
		protected PathMappingMiddleware (OwinMiddleware next, BasePathMappingRules rules) : base (next, rules)
		{
		}
	}
}

