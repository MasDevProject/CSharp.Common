using System;
using System.Threading.Tasks;
using MasDev.IO;
using System.IO;
using Android.Graphics;
using MasDev.Droid.ExtensionMethods;

namespace MasDev.Droid.Utils
{
	public static class BitmapUtils
	{
		static async Task<Bitmap> GetScaledBitmap (string path, int width, int height)
		{
			return await Task.Run(() => {
				var options = new BitmapFactory.Options();
				options.InJustDecodeBounds = true;
				BitmapFactory.DecodeFile(path, options);

				options.InSampleSize =  CalculateInSampleSize (options, width, height);

				options.InJustDecodeBounds = false;
				return BitmapFactory.DecodeFile(path, options);
			});
		}

		static int CalculateInSampleSize (BitmapFactory.Options options, int reqWidth, int reqHeight) 
		{
			// Raw height and width of image
			var height = options.OutHeight;
			var width = options.OutWidth;
			var inSampleSize = 1;

			if (height > reqHeight || width > reqWidth) {

				var halfHeight = height / 2;
				var halfWidth = width / 2;

				// Calculate the largest inSampleSize value that is a power of 2 and keeps both
				// height and width larger than the requested height and width.
				while ((halfHeight / inSampleSize) > reqHeight
					&& (halfWidth / inSampleSize) > reqWidth) {
					inSampleSize *= 2;
				}
			}

			return inSampleSize;
		}

		public static async Task<MimedStream> ScaleToStreamAsync(string filePath, int maxSize)
		{
			var fi = new FileInfo(filePath);
			string fileName = fi.Name;

			Bitmap source;

			using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
				source = await BitmapFactory.DecodeStreamAsync (stream);

			var maxDim = source.Height > source.Width ? source.Height : source.Width;
			var scale = maxDim > maxSize ? (float)maxSize / maxDim : 1f;

			using (source) {
				var resizedBitmap = Bitmap.CreateScaledBitmap (source, (int)(source.Width * scale), (int)(source.Height * scale), true);
				return new MimedStream (await resizedBitmap.AsStreamAsync (Bitmap.CompressFormat.Png), Mime.FromPath (filePath), fileName);
			}
		}
	}
}

