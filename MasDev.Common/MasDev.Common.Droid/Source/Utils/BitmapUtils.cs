using System;
using System.Threading.Tasks;
using MasDev.Common.IO;
using System.IO;
using Android.Graphics;
using MasDev.Common.Droid.ExtensionMethods;

namespace MasDev.Common.Droid
{
	public static class BitmapUtils
	{
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

		public static async Task<Bitmap> GetScaledBitmap(string imagePath, int requiredSize)
		{
			var o = new BitmapFactory.Options ();
			o.InJustDecodeBounds = true;
			await BitmapFactory.DecodeStreamAsync (new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), null, o);

			int scale=1;
			while(o.OutWidth/scale/2 >= requiredSize && o.OutHeight/scale/2 >= requiredSize)
				scale*=2;

			//Decode with inSampleSize
			var o2 = new BitmapFactory.Options();
			o2.InSampleSize = scale;
			return await BitmapFactory.DecodeStreamAsync(new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), null, o2);
		}
	}
}

