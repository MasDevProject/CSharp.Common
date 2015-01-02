using System;
using Android.Content;
using Android.Util;
using Android.Widget;
using Android.Graphics;
using Android.Support.V4.View;
using Android.Views;
using MasDev.Droid.Utils;

namespace MasDev.Droid.Views
{
	/// <summary>
	/// Usage: you have to crate an intance of this object (by code, not in the xml file) and then assign that instance
	/// to a ViewGroup (FrameLayout, LinearLayout, Relativelayout and so on) using viewGriup.AddView(new PagerPositionIndicator()).
	/// You can also pass two views (left and right arrow) and the class will hide/show/handle-click the left/right arrow for you. 
	/// If you don't want to use a left/right arrow, you can pass null as paramenter
	/// 
	/// Example:
	///  layout.xml ----------------------------------
	///  <LinearLayout
	///  	android:orinetation="vertical"
	///		android:layout_width="match_parent"
	///		android:layout_height="wrap_content">
	///		<android.support.v4.view.ViewPager
	///			android:id="@+id/pager"
	///			android:layout_width="match_parent"
	///			android:layout_height="160dp"
	///			android:paddingLeft="16dp"
	///			android:paddingRight="16dp" />
	///		<FrameLayout
	///			android:id="@+id/bottomIndicatorLayout"
	///			android:layout_width="match_parent"
	///			android:layout_height="wrap_content" />
	///	</LinearLayout>
	///
	/// code -----------------------------------------
	/// var pager = View.FindViewById<ViewPager> (Resource.Id.pager);
	/// pager.Adapter = ...
	///
	/// var bottomIndicatorLayout = View.FindViewById<FrameLayout> (Resource.Id.bottomIndicatorLayout);
	/// bottomIndicatorLayout.AddView (new PagerPositionIndicator (Activity, pager, Orientation.Horizontal, _innerFragmentCount, Resource.Drawable.Void_ball, Resource.Drawable.Full_ball, 12, 5, 5, buttonLeftArrow, buttonRightArrow));
	/// 
	/// That's all!
	/// 
	/// </summary>
	public class PagerPositionIndicator : LinearLayout
	{
		Bitmap _voidBallBitmap;
		Bitmap _fullBallBitmap;
		int _innerFragmentIndex;

		public PagerPositionIndicator (Context context, ViewPager pager, Orientation orientation, int pagesNumber, int voidIndicatorResId, int fullIndicatorResId, float indicatorDpDimension, float indicatorDpMarginsTopBottom, float indicatorDpMarginsLeftRight, View btnLeft, View btnRight) : base (context)
		{
			Initialize (context, pager, orientation, pagesNumber, voidIndicatorResId, fullIndicatorResId, indicatorDpDimension, indicatorDpMarginsTopBottom, indicatorDpMarginsLeftRight, btnLeft, btnRight);
		}

		public PagerPositionIndicator (Context context, IAttributeSet attrs) : base (context, attrs)
		{
		}

		public PagerPositionIndicator (Context context, IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle)
		{
		}

		void Initialize (Context context, ViewPager pager, Orientation orientation, int pagesNumber, int voidIndicatorResId, int fullIndicatorResId, float indicatorDpDimension, float indicatorDpMarginsTopBottom, float indicatorDpMarginsLeftRight, View btnLeft, View btnRight)
		{
			_voidBallBitmap = BitmapFactory.DecodeResource (context.Resources, voidIndicatorResId);
			_fullBallBitmap = BitmapFactory.DecodeResource (context.Resources, fullIndicatorResId);

			LayoutParameters = new LayoutParams (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
			SetGravity (GravityFlags.Center);
			Orientation = orientation;

			var dim = ApplicationUtils.ConvertDpToPixel (context, indicatorDpDimension);
			var lp = new LinearLayout.LayoutParams (dim, dim);
			var marginLeftRight = ApplicationUtils.ConvertDpToPixel (context, indicatorDpMarginsLeftRight);
			var marginTopBottom = ApplicationUtils.ConvertDpToPixel (context, indicatorDpMarginsTopBottom);
			lp.SetMargins (marginLeftRight, marginTopBottom, marginLeftRight, marginTopBottom);

			for (var i = 0; i < pagesNumber; i++) {
				var image = new ImageView (context);
				image.SetImageBitmap (pager.CurrentItem == i ? _fullBallBitmap : _voidBallBitmap);
				AddView (image, lp);
			}

			pager.PageSelected += (sender, e) => {
				_innerFragmentIndex = e.Position;

				btnLeft.Visibility = _innerFragmentIndex == 0 ? ViewStates.Invisible : ViewStates.Visible;
				btnRight.Visibility = _innerFragmentIndex == pagesNumber - 1 ? ViewStates.Invisible : ViewStates.Visible;

				for (var i = 0; i < ChildCount; i++) 
					((ImageView)GetChildAt (i)).SetImageBitmap (i == _innerFragmentIndex ? _fullBallBitmap : _voidBallBitmap);
			};

			if (btnLeft != null && btnRight != null) {
				btnRight.Click += delegate {
					_innerFragmentIndex = _innerFragmentIndex + 1 < pagesNumber ? ++_innerFragmentIndex : _innerFragmentIndex;
					pager.SetCurrentItem (_innerFragmentIndex, true);
				};

				btnLeft.Click += delegate {
					_innerFragmentIndex = _innerFragmentIndex > 0 ? --_innerFragmentIndex : _innerFragmentIndex;
					pager.SetCurrentItem (_innerFragmentIndex, true);
				};
			}
		}

		public new void Dispose()
		{
			if (_voidBallBitmap != null) {
				_voidBallBitmap.Dispose ();
				_voidBallBitmap = null;
			}
			if (_fullBallBitmap != null) {
				_fullBallBitmap.Dispose ();
				_fullBallBitmap = null;
			}
			base.Dispose ();
		}
	}
}

