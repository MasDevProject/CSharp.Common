using UIKit;
using CoreGraphics;
using MasDev.iOS.Views.States;

namespace MasDev.iOS.Views
{
	public class EmptyStateSourceView : BaseStateView
	{
		readonly UILabel _lblEmptyMessage;

		private const float Padding = 16;

		public string ImagePath { get; set; }

		public string Message { get; set; }

		public EmptyStateSourceView (CGRect frame) : base(frame)
		{
			_lblEmptyMessage = new UILabel (new CGRect(0, 0, Frame.Width - (Padding * 2), 0));
			_lblEmptyMessage.Lines = 0;
			_lblEmptyMessage.LineBreakMode = UILineBreakMode.WordWrap;
			_lblEmptyMessage.TextAlignment = UITextAlignment.Center;

			_lblEmptyMessage.TextColor = UIColor.LightGray;
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			if (_lblEmptyMessage != null) 
			{
				_lblEmptyMessage.Text = Message;
				_lblEmptyMessage.SizeToFit ();

				_lblEmptyMessage.Center = new CGPoint(Center.X, Bounds.Height / 2);

				AddSubview (_lblEmptyMessage);
			}
		}
	}
}