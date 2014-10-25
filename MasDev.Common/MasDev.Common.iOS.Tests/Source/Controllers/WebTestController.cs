using System;
using MonoTouch.UIKit;
using MasDev.Common.Share.Tests;

namespace MasDev.Common.iOS.Tests
{
	partial class WebTestController : UIViewController
	{
		public WebTestController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			btnFire.TouchDown += async (sender, e) => {
				lblResults.Text = await GoogleAPIs.GetGoogleHomePage();
			};
		}
	}
}
