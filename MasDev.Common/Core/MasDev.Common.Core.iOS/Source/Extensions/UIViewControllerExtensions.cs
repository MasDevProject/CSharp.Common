using UIKit;

namespace MasDev.Common
{
	public static class UIViewControllerExtensions
	{
		public static void AddChildViewController(this UIViewController parent, UIView containerView, UIViewController child)
		{
			parent.AddChildViewController (child);

			child.View.Frame = containerView.Bounds;
			child.View.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			containerView.AddSubview (child.View);

			child.DidMoveToParentViewController (parent);
		}

		public static void RemoveChildViewController(this UIViewController child)
		{
			child.WillMoveToParentViewController (null);
			child.View.RemoveFromSuperview ();
			child.RemoveFromParentViewController ();
		}
	}
}