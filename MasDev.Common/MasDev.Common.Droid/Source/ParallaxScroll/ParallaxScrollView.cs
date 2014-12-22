using Android.Widget;
using System.Collections.Generic;
using Android.Content;
using Android.Util;
using MasDev.Common.Droid;
using Android.Views;
using System;

namespace Nirhart.ParallaxScroll.Views
{
	public class ParallaxScrollView : ScrollView
	{
		private const int DEFAULT_PARALLAX_VIEWS = 1;
		private const float DEFAULT_INNER_PARALLAX_FACTOR = 1.9f;
		private const float DEFAULT_PARALLAX_FACTOR = 1.9f;
		private const float DEFAULT_ALPHA_FACTOR = -1f;
		private int _numOfParallaxViews = DEFAULT_PARALLAX_VIEWS;
		private float _innerParallaxFactor = DEFAULT_PARALLAX_FACTOR;
		private float _parallaxFactor = DEFAULT_PARALLAX_FACTOR;
		private float _alphaFactor = DEFAULT_ALPHA_FACTOR;
		private List<ParallaxedView> parallaxedViews = new List<ParallaxedView> ();

		public ParallaxScrollView (Context context, IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle)
		{
			Init (context, attrs);
		}

		public ParallaxScrollView (Context context, IAttributeSet attrs) : base (context, attrs)
		{
			Init (context, attrs);
		}

		public ParallaxScrollView (Context context) : base (context)
		{
		}

		protected void Init (Context context, IAttributeSet attrs)
		{
			var typeArray = context.ObtainStyledAttributes (attrs, Resource.Styleable.ParallaxScroll);
			this._parallaxFactor = typeArray.GetFloat (Resource.Styleable.ParallaxScroll_parallax_factor, DEFAULT_PARALLAX_FACTOR);
			this._alphaFactor = typeArray.GetFloat (Resource.Styleable.ParallaxScroll_alpha_factor, DEFAULT_ALPHA_FACTOR);
			this._innerParallaxFactor = typeArray.GetFloat (Resource.Styleable.ParallaxScroll_inner_parallax_factor, DEFAULT_INNER_PARALLAX_FACTOR);
			this._numOfParallaxViews = typeArray.GetInt (Resource.Styleable.ParallaxScroll_parallax_views_num, DEFAULT_PARALLAX_VIEWS);
			typeArray.Recycle ();
		}

		protected override void OnFinishInflate ()
		{
			base.OnFinishInflate ();
			MakeViewsParallax ();
		}

		private void MakeViewsParallax ()
		{
			if (!(ChildCount > 0 && GetChildAt (0) is ViewGroup))
				return;
			var viewsHolder = (ViewGroup)GetChildAt (0);
			int numOfParallaxViews = Math.Min (this._numOfParallaxViews, viewsHolder.ChildCount);
			for (int i = 0; i < numOfParallaxViews; i++) {
				var parallaxedView = new ScrollViewParallaxedItem (viewsHolder.GetChildAt (i));
				parallaxedViews.Add (parallaxedView);
			}
		}

		protected override void OnScrollChanged (int l, int t, int oldl, int oldt)
		{
			base.OnScrollChanged (l, t, oldl, oldt);
			float parallax = _parallaxFactor;
			float alpha = _alphaFactor;
			foreach (var parallaxedView in parallaxedViews) {
				parallaxedView.Offset = (float)t / parallax;
				parallax *= _innerParallaxFactor;
				if (alpha != DEFAULT_ALPHA_FACTOR) {
					float fixedAlpha = (t <= 0) ? 1 : (100 / ((float)t * alpha));
					parallaxedView.Alpha = fixedAlpha;
					alpha /= _alphaFactor;
				}
				parallaxedView.AnimateNow ();
			}
		}


		protected class ScrollViewParallaxedItem : ParallaxedView
		{
			public ScrollViewParallaxedItem (View view) : base (view)
			{
			}

			protected override void TranslatePreICS (View view, float offset)
			{
				view.OffsetTopAndBottom ((int)offset - _lastOffset);
				_lastOffset = (int)offset;
			}
		}
	}
}
