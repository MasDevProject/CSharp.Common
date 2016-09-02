using UIKit;
using System.Linq;
using System;

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

		public static TParent GetParent<TParent>(this UIViewController viewController) where TParent : class, IParent
		{
			// Modal

			var parent = viewController.PresentingViewController as TParent;
			if(parent != default(TParent))
				return parent;

			var presentingNavVC = viewController.PresentingViewController as UINavigationController;
			if (presentingNavVC != null && presentingNavVC.TopViewController != null) 
			{
				parent = presentingNavVC.TopViewController as TParent;
				if (parent != default(TParent))
					return parent;
			}

			// Navigation

			if(viewController.NavigationController != null)
			{
				var controllers = viewController.NavigationController.ViewControllers.ToList ();
				var index = controllers.IndexOf (viewController);
				if (controllers.Count > 1) 
				{
					parent = controllers.ElementAt(index - 1) as TParent;
					if(parent != default(TParent))
						return parent;
				}
			}

			// ChildViewController

			parent = viewController.ParentViewController as TParent;
			if (parent != default(TParent))
				return parent;

			throw new Exception("Missing parent implementation");
		}
	}
}