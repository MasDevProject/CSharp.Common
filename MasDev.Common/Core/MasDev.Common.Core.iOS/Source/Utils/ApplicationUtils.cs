using UIKit;

namespace MasDev.iOS.Utils
{
	public static class ApplicationUtils
	{
		static int _deviceIsTablet = -1;

		public static bool DeviceIsTablet()
		{
			if(_deviceIsTablet == -1) {
				_deviceIsTablet = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad ? 1 : 0;
			}
			return _deviceIsTablet == 1;
		}
	}
}

