using System;
using CoreGraphics;
using UIKit;

namespace MasDev.Common
{
	public static class UITextViewExtensions
	{
		public static CGRect FitFrame (this UITextView textView)
		{
			var fixedWidth = textView.Frame.Size.Width;
			var newSize = textView.SizeThatFits(new CGSize(fixedWidth, nfloat.MaxValue));

			var newFrame = textView.Frame;
			newFrame.Size = new CGSize(Math.Max(newSize.Width, fixedWidth), newSize.Height);

			return newFrame;
		}
	}
}