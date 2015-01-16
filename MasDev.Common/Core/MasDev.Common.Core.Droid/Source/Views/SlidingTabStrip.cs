using System;
using Android.Widget;
using Android.Content;
using Android.Util;
using Android.Graphics;
using Android.Views;
using Android.Content.Res;
using MasDev.Common;

namespace MasDev.Droid.Views
{
	sealed class SlidingTabStrip : LinearLayout
	{
		const int DEFAULT_BOTTOM_BORDER_THICKNESS_DIPS = 2;
		const byte DEFAULT_BOTTOM_BORDER_COLOR_ALPHA = 0x26;
		const int SELECTED_INDICATOR_THICKNESS_DIPS = 8;
		const uint DEFAULT_SELECTED_INDICATOR_COLOR = 0xFF33B5E5;

		const int DEFAULT_DIVIDER_THICKNESS_DIPS = 1;
		const byte DEFAULT_DIVIDER_COLOR_ALPHA = 0x20;
		const float DEFAULT_DIVIDER_HEIGHT = 0.5f;

		readonly int mBottomBorderThickness;
		readonly Paint mBottomBorderPaint;

		readonly int mSelectedIndicatorThickness;
		readonly Paint mSelectedIndicatorPaint;

		//readonly int mDefaultBottomBorderColor;

		readonly Paint mDividerPaint;
		readonly float mDividerHeight;

		int mSelectedPosition;
		float mSelectionOffset;

		//SlidingTabLayout.TabColorizer mCustomTabColorizer;
		readonly SimpleTabColorizer mDefaultTabColorizer;
		Color _rowColor = new Color (0, 0, 0);

		public SlidingTabStrip (Context context, IAttributeSet attrs) : base (context, attrs)
		{
			SetWillNotDraw (false);
			float density = Resources.DisplayMetrics.Density;

			TypedArray a = null;
			try {
				a = Context.ObtainStyledAttributes (attrs, Resource.Styleable.SlidingTabStrip);
				if (a.HasValue (Resource.Styleable.SlidingTabStrip_row_color))
					_rowColor = a.GetColor (Resource.Styleable.SlidingTabStrip_row_color, 0);
			} finally {
				if (a != null)
					a.Recycle ();
			}

			var outValue = new TypedValue ();
			Context.Theme.ResolveAttribute (Android.Resource.Attribute.ColorForeground, outValue, true);
			int themeForegroundColor = outValue.Data;

			//mDefaultBottomBorderColor = SetColorAlpha (themeForegroundColor, DEFAULT_BOTTOM_BORDER_COLOR_ALPHA);

			mDefaultTabColorizer = new SimpleTabColorizer ();
			mDefaultTabColorizer.SetIndicatorColors (unchecked((int)DEFAULT_SELECTED_INDICATOR_COLOR));
			mDefaultTabColorizer.SetDividerColors (SetColorAlpha (themeForegroundColor, DEFAULT_DIVIDER_COLOR_ALPHA));

			mBottomBorderThickness = (int)(DEFAULT_BOTTOM_BORDER_THICKNESS_DIPS * density);
			mBottomBorderPaint = new Paint ();
			mBottomBorderPaint.Color = _rowColor;

			mSelectedIndicatorThickness = (int)(SELECTED_INDICATOR_THICKNESS_DIPS * density);
			mSelectedIndicatorPaint = new Paint ();

			mDividerHeight = DEFAULT_DIVIDER_HEIGHT;
			mDividerPaint = new Paint ();
			mDividerPaint.StrokeWidth = DEFAULT_DIVIDER_THICKNESS_DIPS * density;
		}

		public void SetCustomTabColorizer (SlidingTabLayout.TabColorizer customTabColorizer)
		{
			//mCustomTabColorizer = customTabColorizer;
			Invalidate ();
		}

		public void SetSelectedIndicatorColors (Color color)
		{
			//mCustomTabColorizer = null;
			mDefaultTabColorizer.SetIndicatorColors (color);
			Invalidate ();
		}

		public void SetDividerColors (Color color)
		{
			//mCustomTabColorizer = null;
			mDefaultTabColorizer.SetDividerColors (color);
			Invalidate ();
		}

		public void OnViewPagerPageChanged (int position, float positionOffset)
		{
			mSelectedPosition = position;
			mSelectionOffset = positionOffset;
			Invalidate ();
		}

		protected override void OnDraw (Canvas canvas)
		{
			int height = Height;
			int childCount = ChildCount;
			int dividerHeightPx = (int)(Math.Min (Math.Max (0f, mDividerHeight), 1f) * height);
			//var tabColorizer = mCustomTabColorizer ?? mDefaultTabColorizer;

			// Thick colored underline below the current selection
			if (childCount > 0) {
				View selectedTitle = GetChildAt (mSelectedPosition);
				int left = selectedTitle.Left;
				int right = selectedTitle.Right;
				//int color = tabColorizer.GetIndicatorColor(mSelectedPosition);

				if (mSelectionOffset > 0f && mSelectedPosition < (ChildCount - 1)) {
					//int nextColor = tabColorizer.GetIndicatorColor(mSelectedPosition + 1);
//					if (color != nextColor)
//						color = BlendColors(nextColor, color, mSelectionOffset);
						
					var nextTitle = GetChildAt (mSelectedPosition + 1);
					left = (int)(mSelectionOffset * nextTitle.Left + (1.0f - mSelectionOffset) * left);
					right = (int)(mSelectionOffset * nextTitle.Right + (1.0f - mSelectionOffset) * right);
				}

				mSelectedIndicatorPaint.Color = _rowColor;
				canvas.DrawRect (left, height - mSelectedIndicatorThickness, right, height, mSelectedIndicatorPaint);
			}


			canvas.DrawRect (0, height - mBottomBorderThickness, Width, height, mBottomBorderPaint);

			int separatorTop = (height - dividerHeightPx) / 2;
			for (int i = 0; i < childCount - 1; i++) {
				View child = GetChildAt (i);
				mDividerPaint.Color = _rowColor;
				canvas.DrawLine (child.Right, separatorTop, child.Right, separatorTop + dividerHeightPx, mDividerPaint);
			}
		}

		static int SetColorAlpha (int color, byte alpha)
		{
			return Color.Argb (alpha, Color.GetRedComponent (color), Color.GetGreenComponent (color), Color.GetBlueComponent (color));
		}

		static int BlendColors (int color1, int color2, float ratio)
		{
			float inverseRation = 1f - ratio;
			float r = (Color.GetRedComponent (color1) * ratio) + (Color.GetRedComponent (color2) * inverseRation);
			float g = (Color.GetGreenComponent (color1) * ratio) + (Color.GetGreenComponent (color2) * inverseRation);
			float b = (Color.GetBlueComponent (color1) * ratio) + (Color.GetBlueComponent (color2) * inverseRation);
			return Color.Rgb ((int)r, (int)g, (int)b);
		}

		class SimpleTabColorizer : SlidingTabLayout.TabColorizer
		{
			int[] mIndicatorColors;
			int[] mDividerColors;

			public int GetIndicatorColor (int position)
			{
				return mIndicatorColors [position % mIndicatorColors.Length];
			}

			public int GetDividerColor (int position)
			{
				return mDividerColors [position % mDividerColors.Length];
			}

			public void SetIndicatorColors (params int[] colors)
			{
				mIndicatorColors = colors;
			}

			public void SetDividerColors (params int[] colors)
			{
				mDividerColors = colors;
			}
		}
	}
}

