using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using System.Linq;

namespace MasDev.iOS.Views
{
	public class SlidingTabBarView : UIView
	{
		public event Action<int> OnTabChanged = delegate { };

		public UIColor SelectorColor;
		public UIColor TextColor;
		public UIColor BottomViewColor;

		protected Dictionary<string, KeyValuePair<UIView, nfloat>> Tabs;
		protected UIScrollView ScrollView;
		protected UIView SelectorView;
		protected UIView BottomView;
		protected float TabItemPadding = 8f;
		protected float SelectorHeight = 8f;
		protected float BottomViewHeight = 1f;
		protected float InnerPadding = 3f;
		protected List<string> TabNames;

		protected float TabItemHeight { get { return (float) Frame.Height - BottomViewHeight; } }

		public SlidingTabBarView(CGRect frame, List<string> tabNames, UIScrollView scrollView)
		{
			//TODO: fix same tab names issue

			Frame = frame;

			TabNames = tabNames;

			Tabs = new Dictionary<string, KeyValuePair<UIView, nfloat>>();

			// Defaults

			BackgroundColor = UIColor.LightGray;
			SelectorColor = UIColor.White;
			TextColor = UIColor.White;
			BottomViewColor = UIColor.White;

			if (scrollView != null)
			{
				scrollView.DecelerationEnded += (sender, e) => {
					var currentPage = (int)Math.Floor ((scrollView.ContentOffset.X - scrollView.Frame.Width / 2) / scrollView.Frame.Width) + 1;

					PerformAnimation (Tabs [tabNames [currentPage]].Key, currentPage);

					AdjustAlpha(currentPage);
				};
			}

			AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			ComposeView ();

			FillEmptySpace ();
		}

		protected void FillEmptySpace()
		{
			if (TabNames.Count == 0)
				return;

			// Get current width using base size

			var currentWidth = 0f;

			var items = Tabs.Values.ToList ();
			foreach (var item in items)
				currentWidth += (float) item.Value;

			if (currentWidth >= Frame.Width)
				return;

			// Fill entire space

			var additionalPadding = (Frame.Width - currentWidth) / items.Count;

			var actualWidth = currentWidth;
			var additionalWidthSum = 0f;
			var precX = 0f;
			foreach (var item in items)
			{
				var newFrame = item.Key.Frame;
				newFrame.X = precX;
				newFrame.Width = item.Value + additionalPadding;
				item.Key.Frame = newFrame;

				precX += (float) newFrame.Width;

				additionalWidthSum += (float) additionalPadding;
			}

			actualWidth += additionalWidthSum;

			// Update selector

			if (SelectorView != null)
			{
				var index = (int) SelectorView.Tag;
				if (index < 0 || index >= items.Count)
					index = 0;
				
				UIView.Animate(0.3f, () => SelectorView.Frame = FrameForSelector (items [index].Key, index));
			}

			// Update ScrollView

			if(ScrollView != null)
				ScrollView.ContentSize = new CGSize (actualWidth, TabItemHeight);
		}

		protected void ComposeView()
		{
			if (ScrollView != null)
			{
				ScrollView.ContentInset = UIEdgeInsets.Zero;

				return;
			}
			
			ScrollView = new UIScrollView ();
			ScrollView.AlwaysBounceVertical = false;
			ScrollView.ShowsHorizontalScrollIndicator = false;
			ScrollView.ShowsVerticalScrollIndicator = false;
			ScrollView.Bounces = true;
			ScrollView.BackgroundColor = UIColor.Clear;

			ScrollView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			ScrollView.ContentMode = UIViewContentMode.TopRight;

			UIButton tabItemView;
			var count = 0;
			var widthSum = 0f;
			var tabItemFrame = CGRect.Empty;

			foreach (var tabItem in TabNames) {
				if (!Tabs.ContainsKey (tabItem)) {
					tabItemView = new UIButton (new CGRect (
						widthSum,
						0,
						150,
						TabItemHeight));

					var currentIndex = count;

					tabItemView.TouchUpInside += (s, e) => {
						if (OnTabChanged != null)
							OnTabChanged.Invoke (currentIndex);

						PerformAnimation (s as UIView, currentIndex);

						AdjustAlpha (currentIndex);
					};

					tabItemView.BackgroundColor = UIColor.Clear;

					tabItemView.SetTitle (tabItem, UIControlState.Normal);
					tabItemView.SetTitleColor (TextColor, UIControlState.Normal);
					tabItemView.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;

					tabItemFrame = tabItemView.Frame;
					tabItemView.SizeToFit ();
					tabItemFrame.Width = tabItemView.Frame.Width + (TabItemPadding * 4);
					tabItemView.Frame = tabItemFrame;

					count++;

					Tabs.Add (tabItem, new KeyValuePair<UIView, nfloat>(tabItemView, tabItemView.Frame.Width));

					ScrollView.AddSubview (tabItemView);
				} else
					tabItemView = (UIButton)Tabs [tabItem].Key;

				widthSum += (float)tabItemView.Frame.Width;
			}

			if (TabNames.Count > 0)
			{
				if (SelectorView == null)
				{
					SelectorView = new UIView (FrameForSelector (Tabs.First ().Value.Key, 0));

					SelectorView.BackgroundColor = SelectorColor;

					ScrollView.AddSubview (SelectorView);

					AdjustAlpha (0);
				}
			}

			ScrollView.ContentSize = new CGSize (widthSum, TabItemHeight);
			ScrollView.Frame = new CGRect(0, 0, Frame.Width, TabItemHeight);

			BottomView = new UIView (new CGRect (0, TabItemHeight, Frame.Width, BottomViewHeight));
			BottomView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			BottomView.BackgroundColor = BottomViewColor;

			AddSubview (ScrollView);
			AddSubview (BottomView);
		}

		private void PerformAnimation(UIView tabView, int page)
		{
			UIView.Animate(0.3f, () => SelectorView.Frame = FrameForSelector(tabView, page));

			var startX = tabView.Frame.X;
			if (startX > 0) 
			{
				startX = startX / 2;

				var newScrollOffset = ScrollView.ContentOffset.X + tabView.Frame.Width + startX;

				if (startX > ScrollView.ContentOffset.X && newScrollOffset > ScrollView.Bounds.Width)
					startX = newScrollOffset - ScrollView.Bounds.Width + ScrollView.ContentOffset.X;
			}

			if(ScrollView.Bounds.Width < ScrollView.ContentSize.Width)
				ScrollView.SetContentOffset(new CGPoint(startX, 0), true);
		} 

		private CGRect FrameForSelector(UIView selectedTab, int page)
		{
			if (SelectorView != null)
				SelectorView.Tag = page;
					
			return new CGRect(
				selectedTab.Frame.X,
				selectedTab.Frame.Y + selectedTab.Frame.Height - InnerPadding,
				selectedTab.Frame.Width,
				SelectorHeight);
		}

		private void AdjustAlpha(int index)
		{
			UIButton currentView;

			for (int i = 0; i < TabNames.Count; i++)
			{
				currentView = Tabs [TabNames [i]].Key as UIButton;

				if (i == index) 
				{
					currentView.Alpha = 1f;
					currentView.Font = UIFont.BoldSystemFontOfSize (currentView.Font.PointSize);
				}
				else
				{
					currentView.Alpha = 0.7f;
					currentView.Font = UIFont.SystemFontOfSize (currentView.Font.PointSize);
				}
			}
		}
	}
}