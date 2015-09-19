using System;
using UIKit;

namespace MasDev.iOS.Extensions
{
	public static class UIButtonExtensions
	{
		public static void ToCircleButton(this UIButton button, nfloat borderWidth, UIColor borderColor)
		{
			button.Layer.BorderWidth = borderWidth;
			button.Layer.CornerRadius = button.Bounds.Width / 2;
			button.Layer.BorderColor = borderColor.CGColor;
		}
	}
}