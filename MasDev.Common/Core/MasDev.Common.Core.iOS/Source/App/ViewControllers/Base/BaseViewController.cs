using System;
using MasDev.iOS.App.ViewControllers;
using UIKit;
using Foundation;

namespace MasDev.Common
{
	public class BaseViewController : ScrollableViewController
	{
		protected virtual string ViewTitle { get { return string.Empty; } }

		protected bool IsTablet { get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad; } }

		protected BaseViewController (IntPtr handle) : base (handle)
		{
		}

		protected BaseViewController (string nibName, NSBundle bundle) : base (nibName, bundle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			if(NavigationItem != null && !string.IsNullOrWhiteSpace(ViewTitle))
				NavigationItem.Title = ViewTitle;
		}

		protected void PerformSegue(string segue)
		{
			PerformSegue (segue, this);
		}
	}
}