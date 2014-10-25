﻿using Android.Graphics;
using Android.Widget;
using Android.Graphics.Drawables;


namespace MasDev.Common.Droid
{
	public static class ImageViewExtensions
	{
		public static Bitmap GetBitmap(this ImageView iv)
		{
			return ((BitmapDrawable)iv.Drawable).Bitmap;
		}
	}
}

