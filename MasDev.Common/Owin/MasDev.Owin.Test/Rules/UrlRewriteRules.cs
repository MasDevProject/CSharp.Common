using MasDev.Owin.PathMapping;

namespace MasDev.Owin.Test
{
	class UrlRewriteRules : PathMappingRules
	{
		public UrlRewriteRules ()
		{
			When (path => path.MatchesTemplate ("/mimmo/{id}/ciao")).MapTo ("/timmy");
		}
	}
}

