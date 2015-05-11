using MasDev.Common.Owin.Rules;


namespace MasDev.Common.Owin
{
	class RedirectRules : PathMappingRules
	{
		public RedirectRules ()
		{
			WhenStartsWith ("/test").MapTo ("/ciao");
		}
	}
}

