using System;
using Android.Views;
using Android.Content;
using Android.Util;

namespace MasDev.Droid.Views
{
	/// <summary>
	/// Usage: Add it in the xml (or create it by code)
	/// <MasDev.Droid.Views.FlowLayout
	///    android:id="@+id/flowLayout"
	///	   android:layout_width="match_parent"
	///    android:layout_height="wrap_content"/>
	///
	/// Then add subviews by code (xml inner children are not supported):
	/// var flowLayout = FindViewById<FlowLayout>(Resource.Id.flowLayout);
	/// flowLayout.AddView (myView);
	///
	/// </summary>
	public class FlowLayout : ViewGroup {

		int _lineHeight;

		public FlowLayout(Context context): base (context) {}

		public FlowLayout(Context context, IAttributeSet attrs) : base (context, attrs) {}

		protected override void OnMeasure (int widthMeasureSpec, int heightMeasureSpec)
		{
			//if (MeasureSpec.GetMode (widthMeasureSpec) == MeasureSpecMode.Unspecified)
			//    return;

			var width = MeasureSpec.GetSize(widthMeasureSpec) - PaddingLeft - PaddingRight;
			var height = MeasureSpec.GetSize(heightMeasureSpec) - PaddingTop - PaddingBottom;
			var count = ChildCount;
			var lineHeight = 0;

			var xpos = PaddingLeft;
			var ypos = PaddingTop;

			int childHeightMeasureSpec;
			childHeightMeasureSpec = MeasureSpec.GetMode (heightMeasureSpec) == MeasureSpecMode.AtMost ? MeasureSpec.MakeMeasureSpec (height, MeasureSpecMode.AtMost) : MeasureSpec.MakeMeasureSpec (0, MeasureSpecMode.Unspecified);

			for (var i = 0; i < count; i++) {
				var child = GetChildAt(i);
				if (child.Visibility != ViewStates.Gone) {
					var lp = (LayoutParams) child.LayoutParameters;
					child.Measure(MeasureSpec.MakeMeasureSpec(width, MeasureSpecMode.AtMost), childHeightMeasureSpec);
					var childw = child.MeasuredWidth;
					lineHeight = Math.Max(lineHeight, child.MeasuredHeight + lp.vertical_spacing);

					if (xpos + childw > width) {
						xpos = PaddingLeft;
						ypos += lineHeight;
					}

					xpos += childw + lp.horizontal_spacing;
				}
			}
			_lineHeight = lineHeight;

			if (MeasureSpec.GetMode(heightMeasureSpec) == MeasureSpecMode.Unspecified) {
				height = ypos + lineHeight;

			} else if (MeasureSpec.GetMode(heightMeasureSpec) == MeasureSpecMode.AtMost) {
				if (ypos + lineHeight < height) {
					height = ypos + lineHeight;
				}
			}
			SetMeasuredDimension(width, height);
		}

		protected override ViewGroup.LayoutParams GenerateDefaultLayoutParams ()
		{
			return new LayoutParams (1, 1); // default of 1px spacing
		}

		protected override bool CheckLayoutParams (ViewGroup.LayoutParams p)
		{
			if (p is LayoutParams) 
				return true;

			return false;
		}

		protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			var count = ChildCount;
			var width = r - l;
			var xpos = PaddingLeft;
			var ypos = PaddingTop;

			for (var i = 0; i < count; i++) {
				var child = GetChildAt(i);
				if (child.Visibility != ViewStates.Gone) {
					var childw = child.MeasuredWidth;
					var childh = child.MeasuredHeight;
					var lp = (LayoutParams) child.LayoutParameters;
					if (xpos + childw > width) {
						xpos = PaddingLeft;
						ypos += _lineHeight;
					}
					child.Layout(xpos, ypos, xpos + childw, ypos + childh);
					xpos += childw + lp.horizontal_spacing;
				}
			}
		}

		public new class LayoutParams : ViewGroup.LayoutParams 
		{
			public readonly int horizontal_spacing;
			public readonly int vertical_spacing;

			public LayoutParams (int horizontal_spacing, int vertical_spacing) : base (0, 0) 
			{
				this.horizontal_spacing = horizontal_spacing;
				this.vertical_spacing = vertical_spacing;
			}
		}
	}
}
