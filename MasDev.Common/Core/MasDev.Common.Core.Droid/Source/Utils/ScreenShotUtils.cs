using Android.Graphics;
using Android.Views;
using System.IO;
using Android.App;

namespace MasDev.Common
{
	public static class ScreenShotUtils
	{
		/// <summary>
		/// Take a screenshot of a view. Throws exception.
		/// The method returns the Bitmap but, providing the outPutFodlerPath param, it also save the bitmap on the hard drive.
		/// </summary>
		/// <returns>The partial screenshot Bitmap.</returns>
		/// <param name="view">View.</param>
		/// <param name="outPutFodlerPath">Out put fodler path. If null does not save the image as file</param>
		public static Bitmap TakePartialScreenshot (View view, string outPutFodlerPath)
		{
			view.DrawingCacheEnabled = true;
			view.Measure (
				View.MeasureSpec.MakeMeasureSpec (0, MeasureSpecMode.Unspecified),
				View.MeasureSpec.MakeMeasureSpec (0, MeasureSpecMode.Unspecified)
			);
				
			view.BuildDrawingCache (true);

			var bitmap = Bitmap.CreateBitmap(view.DrawingCache);

			if (outPutFodlerPath != null) {
				var outputStream = new FileStream (outPutFodlerPath, FileMode.CreateNew);
				bitmap.Compress (Bitmap.CompressFormat.Jpeg, 100, outputStream);
				outputStream.Flush ();
				outputStream.Close ();
			}

			view.DestroyDrawingCache();

			return bitmap;
		}

		/// <summary>
		/// Take a screenshot the the endtire screen. Throws exception.
		/// The method returns the Bitmap but, providing the outPutFodlerPath param, it also save the bitmap on the hard drive.
		/// </summary>
		/// <returns>The entire screen screenshot Bitmap.</returns>
		/// <param name="activity"></param>
		/// <param name="outPutFodlerPath">Out put fodler path. If null does not save the image as file</param>
		public static Bitmap TakeAllScreenScreenshot (Activity activity, string outPutFodlerPath) 
		{
			var view = activity.Window.DecorView.RootView;
			view.DrawingCacheEnabled = true;
			var bitmap = Bitmap.CreateBitmap (view.DrawingCache);
			view.DrawingCacheEnabled = false;

			if (outPutFodlerPath != null) {
				var outputStream = new FileStream (outPutFodlerPath, FileMode.CreateNew);
				bitmap.Compress (Bitmap.CompressFormat.Jpeg, 100, outputStream);
				outputStream.Flush ();
				outputStream.Close ();
			}

			return bitmap;
		}
	}
}

