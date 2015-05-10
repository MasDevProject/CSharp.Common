using MasDev.Owin.PathMapping;

namespace MasDev.Owin.Test
{
	class RedirectRules : PathMappingRules
	{
		public RedirectRules ()
		{
			When (path => path == "/test").MapTo ("/ciao");
		}
	}
}

