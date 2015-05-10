using System;
using Microsoft.Owin.Hosting;

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
}
