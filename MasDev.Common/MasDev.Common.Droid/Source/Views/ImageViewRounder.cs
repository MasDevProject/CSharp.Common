using System;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace MasDev.Droid.Views
{
	/// <summary>
	/// 
	/// Using this class is pretty easy.
	/// This is an example:
	/// 
	/// var lo = FindViewById<ImageView> (Resource.Id.lo);
	///	var bmp = BitmapFactory.DecodeResource (_context.Resources, Resource.Drawable.green);
	///	lo.SetImageDrawable (new ImageViewRounder(bmp, bmp.Width/2, 0, true));
	/// 
	/// NB: for a circle image, the radius has to be btm.Width / 2
	/// NB: describing the "effect" parameter is to hard, so, try it! :)
	/// 
	/// </summary>
	public class ImageViewRounder : Drawable
	{
		bool _useGradientOverlay;
		float _cornerRadius;
		RectF _rect = new RectF ();
		BitmapShader _bitmapShader;
		readonly Paint _paint;
		int _margin;

		public ImageViewRounder (Bitmap bitmap, float cornerRadius = 5, int margin = 0, bool withEffect = true)
		{
			_useGradientOverlay = withEffect;
			_cornerRadius = cornerRadius;

			_bitmapShader = new BitmapShader (bitmap, Shader.TileMode.Clamp, Shader.TileMode.Clamp);

			_paint = new Paint { AntiAlias = true };
			_paint.SetShader (_bitmapShader);

			_margin = margin;
		}

		protected override void OnBoundsChange (Rect bounds)
		{
			base.OnBoundsChange (bounds);
			_rect.Set (_margin, _margin, bounds.Width () - _margin, bounds.Height () - _margin);

			if (_useGradientOverlay) {
				var colors = new [] { 0, 0, 0x7f000000 };
				var pos = new [] { 0.0f, 0.7f, 1.0f };
				var vignette = new RadialGradient (_rect.CenterX (),
					_rect.CenterY () * 1.0f / 0.7f,
					_rect.CenterX () * 1.3f,
					colors,
					pos, Shader.TileMode.Clamp);

				var oval = new Matrix ();
				oval.SetScale (1.0f, 0.7f);
				vignette.SetLocalMatrix (oval);

				_paint.SetShader (new ComposeShader (_bitmapShader, vignette, PorterDuff.Mode.SrcOver));
			}
		}

		public override void Draw (Canvas canvas)
		{
			canvas.DrawRoundRect (_rect, _cornerRadius, _cornerRadius, _paint);
		}

		public override int Opacity {
			get {
				return (int)Format.Translucent;
			}
		}

		public override void SetAlpha (int alpha)
		{
			_paint.Alpha = alpha;
		}

		public override void SetColorFilter (ColorFilter cf)
		{
			_paint.SetColorFilter (cf);
		}
	}
}

