using UIKit;
using CoreGraphics;

namespace MasDev.iOS.Views.States
{
	public class BaseStateView : UIView
	{
		public BaseStateView (CGRect frame) : base(frame)
		{
			AutoresizingMask = 
				UIViewAutoresizing.FlexibleWidth |
				UIViewAutoresizing.FlexibleHeight |
				UIViewAutoresizing.FlexibleBottomMargin |
				UIViewAutoresizing.FlexibleTopMargin;

			BackgroundColor = UIColor.White;
		}
	}
}