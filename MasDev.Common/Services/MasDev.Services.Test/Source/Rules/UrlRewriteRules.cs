using MasDev.Services.Middlewares;


namespace MasDev.Services
{
	class UrlRewriteRules : PathMappingRules
	{
		public UrlRewriteRules ()
		{
			WhenMatches ("/mimmo/{id}/ciao").MapTo ("/timmy");
		}
	}
}

