using Microsoft.Owin;

namespace MasDev.Owin
{
	public abstract class RuledMiddleware<TRules, TRule, TRulePredicate> : OwinMiddleware 
		where TRules : Rules<TRule, TRulePredicate>
		where TRule : Rule<TRulePredicate>, new()
	{
		public TRules Rules { get; private set; }

		protected RuledMiddleware (TRules rules, OwinMiddleware next) : base (next)
		{
			Rules = rules;
		}
	}
}

