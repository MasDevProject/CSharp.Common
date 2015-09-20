using CoreGraphics;
using UIKit;

namespace MasDev.iOS.Views.States
{
	public class BaseEmptyStateView : BaseStateView
	{
		UILabel _lblMessage;

		public BaseEmptyStateView (CGRect frame, string emptyMessage) : base(frame)
		{
			_lblMessage = new UILabel ();
			_lblMessage.Text = emptyMessage;
			_lblMessage.Lines = 0;
			_lblMessage.LineBreakMode = UILineBreakMode.WordWrap;
			_lblMessage.TextAlignment = UITextAlignment.Center;
			_lblMessage.TextColor = UIColor.LightGray;

			_lblMessage.SizeToFit ();
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			if (_lblMessage != null)
			{
				_lblMessage.RemoveFromSuperview ();

				_lblMessage.Center = Center;

				AddSubview (_lblMessage);
			}
		}
	}
}