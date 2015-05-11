using MasDev.Common.Owin.Rules;


namespace MasDev.Common.Owin
{
	class UrlRewriteRules : PathMappingRules
	{
		public UrlRewriteRules ()
		{
			WhenMatches ("/mimmo/{id}/ciao").MapTo ("/timmy");
		}
	}
}

