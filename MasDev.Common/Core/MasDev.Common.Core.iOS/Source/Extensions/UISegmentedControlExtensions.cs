using UIKit;
using MasDev.iOS.Utils;

namespace MasDev.iOS.Extensions
{
	public static class UISegmentedControlExtensions
	{
		public static void InsertMultilineTitle(this UISegmentedControl segmentedControl, string title, int index, bool animated)
		{
			var lblTitle = new UILabel ();
			lblTitle.TextColor = segmentedControl.TintColor;
			lblTitle.BackgroundColor = UIColor.Clear;
			lblTitle.TextAlignment = UITextAlignment.Center;
			lblTitle.LineBreakMode = UILineBreakMode.WordWrap;
			lblTitle.Lines = 2;

			lblTitle.Text = title;

			lblTitle.SizeToFit ();

			segmentedControl.InsertSegment (UIImageUtils.FromLayer(lblTitle.Layer), index, animated);
		}
	}
}