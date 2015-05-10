using Microsoft.Owin;

namespace MasDev.Owin
{
	public abstract class RuledMiddleware<TRules, TRule> : OwinMiddleware 
		where TRules : OwinMiddlewareRules<TRule>
		where TRule : OwinMiddlewareRule, new()
	{
		public TRules Rules { get; private set; }

		protected RuledMiddleware (TRules rules, OwinMiddleware next) : base (next)
		{
			Rules = rules;
			Rules.Validate ();
		}
	}
}

