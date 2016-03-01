using UIKit;

namespace MasDev.Common
{
	public static class UIViewControllerExtensions
	{
		public static void AddChildViewController(this UIViewController container, UIView containerView, UIViewController target)
		{
			container.AddChildViewController (target);

			target.View.Frame = containerView.Bounds;
			target.View.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			containerView.AddSubview (target.View);
		}
	}
}