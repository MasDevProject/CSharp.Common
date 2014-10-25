using System;
using System.Collections.Generic;
using MonoTouch.UIKit;
using MasDev.Common.Dependency;
using MasDev.Common.iOS;
using System.IO;
using MasDev.Common.Share.Tests;

namespace MasDev.Common.iOS.Tests
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			Injector.InitializeWith (new BaseDependencyContainer (), new List<IDependencyConfigurator> () { 
				new iOSDependencyConfigurator (), 
				new CoreDependencyConfigurator (Path.Combine (Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "test.db")) 
			});
			RepoManager.InitializeAsync ().Wait();

			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}
