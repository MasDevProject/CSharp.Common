using System;
using Android.App;
using Android.Runtime;
using System.Collections.Generic;
using System.IO;
using MasDev.Common.Dependency;
using MasDev.Common.Share.Tests;
using MasDev.Common.Droid;
using MasDev.Common.Droid.Utils;

namespace MasDev.Common.Droid.Tests
{
	[Application]
	public sealed class TestApplication : Application
	{
		public TestApplication (IntPtr handle, JniHandleOwnership transfer) : base (handle, transfer)
		{
		}

		public override async void OnCreate ()
		{
			// do any initialisation you want here (for example initialising properties)
			Injector.InitializeWith (new BaseDependencyContainer (), new List<IDependencyConfigurator> { 
				new AndroidDependencyConfigurator (), 
				new CoreDependencyConfigurator (Path.Combine (Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "test.db")) 
			});

			await RepoManager.InitializeAsync ();
			ApplicationUtils.Context = this;
		}
	}
}
