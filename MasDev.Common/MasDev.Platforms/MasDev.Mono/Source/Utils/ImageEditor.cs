using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System;


namespace MasDev.Utils
{
	public static class ImageEditor
	{
		public static MemoryStream RoundCorners (Stream sourceImageStream)
		{
			var img = Image.FromStream (sourceImageStream);
			int height = img.Height;
			int width = img.Width;

			int size = Math.Min (height, width);
			bool isWidthSmaller = size == width;
			int delta = Math.Abs (height - width) / 16;

			var cropRectangle = new Rectangle (isWidthSmaller ? 0 : delta, isWidthSmaller ? delta : 0, size, size);
			var bmp = new Bitmap (size, size);
			using (var gp = new GraphicsPath ())
			{
				gp.AddEllipse (cropRectangle);
				using (var gr = Graphics.FromImage (bmp))
				{
					gr.SetClip (gp);
					gr.DrawImage (img, Point.Empty);
				}
			}

			var destStream = new MemoryStream ();
			bmp.Save (destStream, ImageFormat.Png);
			return destStream;
		}
	}
}

