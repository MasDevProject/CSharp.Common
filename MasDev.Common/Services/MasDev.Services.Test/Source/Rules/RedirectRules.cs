using MasDev.Services.Middlewares;

namespace MasDev.Services
{
	class RedirectRules : PathMappingRules
	{
		public RedirectRules ()
		{
			WhenStartsWith ("/test").MapTo ("/ciao");
		}
	}
}

