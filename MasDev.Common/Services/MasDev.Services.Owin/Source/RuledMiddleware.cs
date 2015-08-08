using Microsoft.Owin;

namespace MasDev.Services.Middlewares
{
	public abstract class RuledMiddleware<TRules, TRule> : OwinMiddleware 
		where TRules : OwinMiddlewareRules<TRule>
		where TRule : OwinMiddlewareRule, new()
	{
		public TRules Rules { get; private set; }

		protected RuledMiddleware (OwinMiddleware next, TRules rules) : base (next)
		{
			Rules = rules;
			Rules.Validate ();
		}
	}
}

