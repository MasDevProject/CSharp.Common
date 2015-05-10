using MasDev.Owin.PathMapping;

namespace MasDev.Owin.Test
{
	class UrlRewriteRules : PathMappingRules
	{
		public UrlRewriteRules ()
		{
			WhenMatches ("/mimmo/{id}/ciao").MapTo ("/timmy");
		}
	}
}

