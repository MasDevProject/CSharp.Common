using MasDev.Owin.PathMapping;

namespace MasDev.Owin.Test
{
	class RedirectRules : PathMappingRules
	{
		public RedirectRules ()
		{
			WhenStartsWith ("/test").MapTo ("/ciao");
		}
	}
}

