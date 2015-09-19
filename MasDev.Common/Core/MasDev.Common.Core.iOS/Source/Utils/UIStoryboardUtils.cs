using UIKit;

namespace MasDev.iOS.Utils
{
	public static class UIStoryboardUtils
	{
		public static T InstantiateViewController <T>(UIViewController controller, string storyboardIdentifier) where T : UIViewController
		{
			return controller.Storyboard.InstantiateViewController (storyboardIdentifier) as T;
		}
	}
}