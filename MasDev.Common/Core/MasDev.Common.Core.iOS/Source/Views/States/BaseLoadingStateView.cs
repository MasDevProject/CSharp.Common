using UIKit;
using CoreGraphics;

namespace MasDev.iOS.Views.States
{
	public class BaseLoadingStateView : BaseStateView
	{
		readonly UIActivityIndicatorView _activityIndicator;

		public BaseLoadingStateView (CGRect frame) : base(frame)
		{
			_activityIndicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.Gray);
			_activityIndicator.StartAnimating ();
			_activityIndicator.HidesWhenStopped = true;
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			if (_activityIndicator != null)
			{
				_activityIndicator.RemoveFromSuperview ();

				_activityIndicator.Center = Center;

				AddSubview (_activityIndicator);
			}
		}
	}
}