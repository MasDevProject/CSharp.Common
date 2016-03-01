using System;
using UIKit;

namespace MasDev.iOS.Extensions
{
	public static class UIImageViewExtensions
	{
		public static void SetImageColor(this UIImageView imageView, UIColor color)
		{
			imageView.TintColor = color;
			imageView.Image = imageView.Image.ImageWithRenderingMode (UIImageRenderingMode.AlwaysTemplate);
		}

		public static void RoundImageView(this UIImageView imageView, nfloat radius)
		{
			imageView.Layer.CornerRadius = radius;
			imageView.Layer.MasksToBounds = true;
		}

		public static void ToCircleCorners(this UIImageView imageView)
		{
			imageView.RoundImageView (imageView.Frame.Width / 2);
		}

		public static void SetImageWithTransition(this UIImageView imageView, UIImage image, 
			double duration, UIViewAnimationOptions animationOptions = UIViewAnimationOptions.TransitionCrossDissolve)
		{
			if (imageView == null || image == null)
				return;
			
			UIView.Transition (
				imageView,
				duration,
				animationOptions,
				() => 
				{
					imageView.Image = image;
				},
				null);
		}

		public static void FadeInImage(this UIImageView imageView, UIImage image, double duration)
		{
			if (imageView == null || image == null)
				return;

			imageView.Alpha = 0;
			imageView.Image = image;

			UIView.Animate (duration, () => {
				imageView.Alpha = 1;
			});
		}
	}
}