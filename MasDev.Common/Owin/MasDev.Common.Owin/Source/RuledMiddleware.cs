using Microsoft.Owin;
using MasDev.Common.Owin.Rules;

namespace MasDev.Common.Owin.Middlewares
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

