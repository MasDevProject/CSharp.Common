using System;
using UIKit;
using CoreGraphics;
using CoreImage;

namespace MasDev.iOS.Extensions
{
	public static class UIImageExtensions
	{
		// resize the image to be contained within a maximum width and height, keeping aspect ratio
		public static UIImage ScaleImage(this UIImage sourceImage, float maxWidth, float maxHeight)
		{
			var sourceSize = sourceImage.Size;
			var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
			if (maxResizeFactor > 1) return sourceImage;
			var width = (float) (maxResizeFactor * sourceSize.Width);
			var height = (float) (maxResizeFactor * sourceSize.Height);

			UIGraphics.BeginImageContextWithOptions(new CGSize(width, height), false, 2.0f);

			sourceImage.Draw(new CGRect(0, 0, width, height));
			var resultImage = UIGraphics.GetImageFromCurrentImageContext();

			UIGraphics.EndImageContext();
			return resultImage;
		}

		public static UIImage Blur(this UIImage image, float blurRadius = 25f)
		{
			if (image != null)
			{
				// Create a new blurred image.
				var imageToBlur = new CIImage (image);
				var blur = new CIGaussianBlur ();
				blur.Image = imageToBlur;
				blur.Radius = blurRadius;
				var blurImage = blur.OutputImage;
				var context = CIContext.FromOptions (new CIContextOptions { UseSoftwareRenderer = false });
				var cgImage = context.CreateCGImage (blurImage, new CGRect (new CGPoint (0, 0), image.Size));
				var newImage = UIImage.FromImage (cgImage);
				// Clean up
				imageToBlur.Dispose ();
				context.Dispose ();
				blur.Dispose ();
				blurImage.Dispose ();
				cgImage.Dispose ();
				return newImage;
			}

			return null;
		}
	}
}