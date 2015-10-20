using System;
using UIKit;
using CoreGraphics;
using Foundation;

namespace MasDev.iOS.Extensions
{
	public static class UILabelExtensions
	{
		public static void AdjustFrameWithText(this UILabel label)
		{
			label.Lines = 0;
			label.SizeToFit ();
		}

		// Compute new Frame with max specified height
		public static void AdjustFrameWithText(this UILabel label, float maxHeight = 960f)
		{
			label.Lines = 0;

			var width = label.Frame.Width; 
			CGSize size = ((NSString)label.Text).StringSize(
				label.Font, 
				constrainedToSize: new CGSize(width, maxHeight),
				lineBreakMode:UILineBreakMode.WordWrap);

			var labelFrame = label.Frame;
			labelFrame.Size = new CGSize(width, size.Height);
			label.Frame = labelFrame;
		}
	}
}