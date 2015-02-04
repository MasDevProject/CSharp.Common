using UIKit;
using Foundation;

namespace MasDev.iOS.Utils
{
	public static class ApplicationUtils
	{
		public static bool DeviceIsTablet {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad; }
		}

		public static void DisableScreenLock(bool disabled)
		{
			UIApplication.SharedApplication.IdleTimerDisabled = disabled;
		}

		public static void ShowNetworkActivityIndicator(bool visible)
		{
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = visible;
		}
	}
}