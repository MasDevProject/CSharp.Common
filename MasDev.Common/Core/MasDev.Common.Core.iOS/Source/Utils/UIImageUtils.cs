using UIKit;
using Foundation;
using System.IO;
using CoreAnimation;

namespace MasDev.iOS.Utils
{
	public static class UIImageUtils
	{
		public static UIImage FromData(byte[] data)
		{
			UIImage image = null;

			if (data == null)
				return image;

			using (var imageData = NSData.FromArray (data))
			{
				return UIImage.LoadFromData (imageData);
			}
		}

		public static UIImage FromStream(Stream stream)
		{
			UIImage image = null;

			if (stream == null)
				return image;

			using (var imageData = NSData.FromStream (stream))
			{
				return UIImage.LoadFromData (imageData);
			}
		}

		public static UIImage FromLayer(CALayer layer)
		{
			UIGraphics.BeginImageContextWithOptions(layer.Frame.Size, false, 0);
			layer.RenderInContext(UIGraphics.GetCurrentContext());
			UIImage outputImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return outputImage;
		}
	}
}