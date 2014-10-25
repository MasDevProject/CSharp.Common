using Android.Content;
using Android.Widget;
using Android.Util;
using Android.Graphics;
using Android.Graphics.Drawables;
using System;

namespace MasDev.Common.Droid.Views
{
	public class RoundedImageView : ImageView 
	{
	
		public RoundedImageView(Context context) : base(context)
		{
		}

		public RoundedImageView(Context context, IAttributeSet attrs) : base(context, attrs)
		{
		}

		public RoundedImageView (Context context, IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle)
		{
		}

		protected override void OnDraw (Canvas canvas)
		{
			Drawable drawable = Drawable;

			if (drawable == null) {
				return;
			}

			if (Width == 0 || Height == 0) {
				return;
			}
			Bitmap b = ((BitmapDrawable) drawable).Bitmap;
			if (b == null)
				return; 
			Bitmap bitmap = b.Copy(Bitmap.Config.Argb8888, true);

			int w = Width;

			Bitmap roundBitmap = GetCroppedBitmap(bitmap, w);
			canvas.DrawBitmap(roundBitmap, 0, 0, null);

			bitmap.Recycle ();
			roundBitmap.Recycle ();
		}


		public static Bitmap GetCroppedBitmap(Bitmap bmp, int radius) {
			Bitmap sbmp;

			if (bmp.Width != radius || bmp.Height != radius) {
				float smallest = Math.Min(bmp.Width, bmp.Height);
				float factor = smallest / radius;
				sbmp = Bitmap.CreateScaledBitmap(bmp, (int)(bmp.Width / factor), (int)(bmp.Height / factor), false);
			} else {
				sbmp = bmp;
			}
			Bitmap output = Bitmap.CreateBitmap(radius, radius, Bitmap.Config.Argb8888);
			var canvas = new Canvas(output);

			var paint = new Paint();
			var rect = new Rect(0, 0, radius, radius);

			paint.AntiAlias = (true);
			paint.FilterBitmap = (true);
			paint.Dither = (true);
			canvas.DrawARGB(0, 0, 0, 0);
			paint.Color = (Color.ParseColor("#BAB399"));
			canvas.DrawCircle(radius / 2, radius / 2 , radius /2, paint);
			paint.SetXfermode (new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
			canvas.DrawBitmap(sbmp, rect, rect, paint);

			sbmp.Recycle ();
			return output;
		}

	}
}

