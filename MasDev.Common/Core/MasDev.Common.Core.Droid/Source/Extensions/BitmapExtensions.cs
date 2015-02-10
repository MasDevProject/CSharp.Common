using Android.Graphics;
using System.IO;
using System.Threading.Tasks;
using System;
using Android.Content;
using Android.Support.V8.Renderscript;

namespace MasDev.Droid.ExtensionMethods
{
	public static class BitmapExtensions
	{
		public static async Task<byte[]> AsByteArray (this Bitmap bitmap, Bitmap.CompressFormat compressFormat)
		{
			using (var stream = new MemoryStream ()) {
				await bitmap.CompressAsync (compressFormat, 100, stream);
				return stream.ToArray ();
			}
		}

		public static async Task<Stream> AsStreamAsync (this Bitmap bitmap, Bitmap.CompressFormat compressFormat)
		{ 
			var guid = Guid.NewGuid ().ToString ();
			var tmpPath = System.IO.Path.GetTempPath () + "/" + guid;
			using (var writeStream = new FileStream (tmpPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
				await bitmap.CompressAsync (compressFormat, 100, writeStream);

			return new FileStream (tmpPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		}

		public static async Task<string> AsBase64String(this Bitmap bitmap)
		{
			return Convert.ToBase64String (await bitmap.AsByteArray (Bitmap.CompressFormat.Png));
		}

		/// <summary>
		/// Returns a blurred image. Usually radius value is between 5 and 25
		/// </summary>
		/// <returns>The blur image.</returns>
		/// <param name="input">Input.</param>
		/// <param name="context">Context.</param>
		/// <param name="radius">Radius.</param>
		public static Bitmap AsBlurImage (this Bitmap input, Context context, int radius)
		{
			var rsScript = RenderScript.Create (context);
			var alloc = Allocation.CreateFromBitmap (rsScript, input);

			var blur = ScriptIntrinsicBlur.Create (rsScript, alloc.Element);
			blur.SetRadius (radius);
			blur.SetInput (alloc);

			var result = Bitmap.CreateBitmap (input.Width, input.Height, input.GetConfig ());
			var outAlloc = Allocation.CreateFromBitmap (rsScript, result);
			blur.ForEach (outAlloc);
			outAlloc.CopyTo (result);

			rsScript.Destroy ();
			return result;
		}
	}
}

