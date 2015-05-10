using System;
using Microsoft.Owin.Hosting;
using Owin;
using MasDev.Owin.PathMapping;

namespace MasDev.Owin.Test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			const string url = "http://+:2034";

			using (WebApp.Start<Startup> (url)) {
				Console.WriteLine ("Server started at " + url + "\n[press a key to exit]");
				Console.ReadLine ();
				Console.WriteLine ("Server closed");
			}
		}
	}


	class Startup
	{
		public void Configuration (IAppBuilder builder)
		{
			builder.UseSimpleInjectorMiddleware ();
			builder.UseRedirectMiddleware (new RedirectRules ());
		}
	}

	class RedirectRules : PathMappingRules
	{
		public RedirectRules ()
		{
			When (path => {
				return path == "/test";
			}).MapTo ("ciao");
		}
	}
}
