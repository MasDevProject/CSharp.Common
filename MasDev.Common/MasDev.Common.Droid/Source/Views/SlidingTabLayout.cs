using Android.Support.V4.View;
using Android.Content;
using Android.Widget;
using Android.Util;
using Android.Views;
using Android.Graphics;
using Android.OS;
using System;

namespace MasDev.Common.Droid.Views
{
	public sealed class SlidingTabLayout : HorizontalScrollView
	{
		public interface TabColorizer 
		{
			int GetIndicatorColor (int position);
			int GetDividerColor (int position);
		}

		const int TITLE_OFFSET_DIPS = 24;
		const int TAB_VIEW_PADDING_DIPS = 16;
		const int TAB_VIEW_TEXT_SIZE_SP = 12;
		readonly int mTitleOffset;

		int mTabViewLayoutId;
		int mTabViewTextViewId;

		ViewPager mViewPager;
		ViewPager.IOnPageChangeListener mViewPagerPageChangeListener;
		readonly SlidingTabStrip mTabStrip;

		public SlidingTabLayout(Context context) : this (context, null, 0)  {}

		public SlidingTabLayout(Context context, IAttributeSet attrs) : this(context, attrs, 0)  {}

		public SlidingTabLayout(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) 
		{
			HorizontalScrollBarEnabled = false;
			FillViewport = true;
			mTitleOffset = (int) (TITLE_OFFSET_DIPS * Resources.DisplayMetrics.Density);
			mTabStrip = new SlidingTabStrip (context);
			AddView(mTabStrip, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);	
		}

		public void SetCustomTabColorizer(TabColorizer tabColorizer) 
		{
			mTabStrip.SetCustomTabColorizer(tabColorizer);
		}

		public void SetSelectedIndicatorColors(params int[] colors) 
		{
			mTabStrip.SetSelectedIndicatorColors(colors);
		}

		public void SetDividerColors(params int[] colors) 
		{
			mTabStrip.SetDividerColors(colors);
		}

		public void SetOnPageChangeListener(ViewPager.IOnPageChangeListener listener) 
		{
			mViewPagerPageChangeListener = listener;
		}

		public void SetCustomTabView(int layoutResId, int textViewId) 
		{
			mTabViewLayoutId = layoutResId;
			mTabViewTextViewId = textViewId;
		}

		public void SetViewPager(ViewPager viewPager) 
		{
			mTabStrip.RemoveAllViews();
			mViewPager = viewPager;
			if (viewPager != null) {
				viewPager.SetOnPageChangeListener(new InternalViewPagerListener(mViewPager, mTabStrip, mViewPagerPageChangeListener, ScrollToTab));
				PopulateTabStrip();
			}
		}

		public TextView CreateDefaultTabView(Context context) 
		{
			var textView = new TextView(context);
			textView.Gravity = GravityFlags.Center;
			textView.SetTextSize(ComplexUnitType.Sp, TAB_VIEW_TEXT_SIZE_SP);
			textView.Typeface = Typeface.DefaultBold;

			if (Build.VERSION.SdkInt >= Build.VERSION_CODES.Honeycomb) {
				var outValue = new TypedValue();
				Context.Theme.ResolveAttribute(Android.Resource.Attribute.SelectableItemBackground, outValue, true);
				textView.SetBackgroundResource(outValue.ResourceId);
			}

			if (Build.VERSION.SdkInt >= Build.VERSION_CODES.IceCreamSandwich)
				textView.SetAllCaps(true);

			int padding = (int) (TAB_VIEW_PADDING_DIPS * Resources.DisplayMetrics.Density);
			textView.SetPadding(padding, padding, padding, padding);

			return textView;
		}

		void PopulateTabStrip() 
		{
			var adapter = mViewPager.Adapter;
			var tabClickListener = new TabClickListener (mViewPager, mTabStrip);

			for (int i = 0; i < adapter.Count; i++) 
			{
				View tabView = null;
				TextView tabTitleView = null;

				if (mTabViewLayoutId != 0) {
					tabView = LayoutInflater.From(Context).Inflate(mTabViewLayoutId, mTabStrip, false);
					tabTitleView = (TextView) tabView.FindViewById(mTabViewTextViewId);
				}

				if (tabView == null)
					tabView = CreateDefaultTabView(Context);

				if (tabTitleView == null &&  tabView.GetType () == typeof(TextView))
					tabTitleView = (TextView) tabView;

				tabTitleView.Text = (adapter.GetPageTitle(i));
				tabView.SetOnClickListener(tabClickListener);

				mTabStrip.AddView(tabView);
			}
		}

		protected override void OnAttachedToWindow ()
		{
			base.OnAttachedToWindow ();
			if (mViewPager != null)  ScrollToTab(mViewPager.CurrentItem, 0);
		}

		void ScrollToTab (int tabIndex, int positionOffset) 
		{
			int tabStripChildCount = mTabStrip.ChildCount;
			if (tabStripChildCount == 0 || tabIndex < 0 || tabIndex >= tabStripChildCount)
				return;

			var selectedChild = mTabStrip.GetChildAt(tabIndex);
			if (selectedChild != null) {
				int targetScrollX = selectedChild.Left + positionOffset;

				if (tabIndex > 0 || positionOffset > 0)
					targetScrollX -= mTitleOffset;

				ScrollTo(targetScrollX, 0);
			}
		}

		class TabClickListener : Java.Lang.Object, View.IOnClickListener 
		{
			readonly ViewPager mViewPager;
			readonly SlidingTabStrip mTabStrip;

			public TabClickListener(ViewPager viewPager,  SlidingTabStrip tabStrip)
			{
				mViewPager = viewPager;
				mTabStrip = tabStrip;
			}

			public void OnClick (View v)
			{
				for (int i = 0; i < mTabStrip.ChildCount; i++) {
					if (v == mTabStrip.GetChildAt (i)) {
						mViewPager.SetCurrentItem (i, true);
						return;
					}
				}
			}
		}

		sealed class InternalViewPagerListener : Java.Lang.Object, ViewPager.IOnPageChangeListener 
		{
			int _mScrollState;
			readonly ViewPager _mViewPager;
			readonly SlidingTabStrip _mTabStrip;
			readonly ViewPager.IOnPageChangeListener _mViewPagerPageChangeListener;
			readonly Action<int,int> _scrollToTab;

			public InternalViewPagerListener (ViewPager viewPager, SlidingTabStrip tabStrip, ViewPager.IOnPageChangeListener viewPagerPageChangeListener, Action<int,int> scrollToTab)
			{
				_mViewPager = viewPager;
				_mTabStrip = tabStrip;
				_mViewPagerPageChangeListener = viewPagerPageChangeListener;
				_scrollToTab = scrollToTab;
			}

			public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels) {
				int tabStripChildCount = _mTabStrip.ChildCount;
				if ((tabStripChildCount == 0) || (position < 0) || (position >= tabStripChildCount))
					return;

				_mTabStrip.OnViewPagerPageChanged(position, positionOffset);

				View selectedTitle = _mTabStrip.GetChildAt(position);
				int extraOffset = selectedTitle != null ? ((int)(positionOffset * selectedTitle.Width)) : 0;
				_scrollToTab(position, extraOffset);

				if (_mViewPagerPageChangeListener != null)
					_mViewPagerPageChangeListener.OnPageScrolled(position, positionOffset, positionOffsetPixels);
			}


			public void OnPageScrollStateChanged(int state) 
			{
				_mScrollState = state;

				if (_mViewPagerPageChangeListener != null)
					_mViewPagerPageChangeListener.OnPageScrollStateChanged(state);
			}


			public void OnPageSelected(int position) 
			{
				if (_mScrollState == ViewPager.ScrollStateIdle) {
					_mTabStrip.OnViewPagerPageChanged(position, 0f);
					_scrollToTab(position, 0);
				}

				if (_mViewPagerPageChangeListener != null)
					_mViewPagerPageChangeListener.OnPageSelected(position);
			}
		}
	}
}

