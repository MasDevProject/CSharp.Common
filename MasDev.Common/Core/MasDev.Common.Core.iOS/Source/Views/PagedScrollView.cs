using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using System.Linq;
using MasDev.iOS.Views.Fragments;

namespace MasDev.iOS.Views
{
	public class PagedScrollView : UIView
	{
		private UIScrollView _scrollView;
		private List<IFragmentView> _fragmentViews;

		public UIScrollView ScrollView
		{
			get
			{
				if (_scrollView == null)
					_scrollView = CreateScrollView ();

				return _scrollView;
			}
		}

		public List<IFragmentView> Views
		{
			set
			{
				PopulateFragmentViews (value);
				RefreshScrollViewContent ();
			}
		}

		public List<string> Titles
		{
			get
			{
				if (_fragmentViews == null)
					return new List<string>();
				
				return _fragmentViews.Select (f => f.Title).ToList ();
			}
		}

		public PagedScrollView(CGRect frame) : base(frame)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			// Fix ScrollView's content to match bounds

			int count = 0;
			var frame = CGRect.Empty;

			foreach (var subView in ScrollView.Subviews)
			{
				frame = subView.Frame;
				frame.Size = Bounds.Size;
				frame.Location = new CGPoint (Bounds.Width * count, 0);
				subView.Frame = frame;

				count++;
			}

			ScrollView.Frame = Bounds;
			ScrollView.ContentSize = new CGSize(Bounds.Width * (count == 0 ? 1 : count), Bounds.Height);

			// Fix contentOffset to current page

			var currentPage = (int)Math.Floor ((ScrollView.ContentOffset.X - ScrollView.Frame.Width / 2) / ScrollView.Frame.Width) + 1;
			ScrollView.SetContentOffset (new CGPoint (ScrollView.Bounds.Width * currentPage, 0), true);
		}

		protected UIScrollView CreateScrollView()
		{
			_scrollView = new UIScrollView (Bounds);

			_scrollView.AlwaysBounceVertical = false;
			_scrollView.ShowsHorizontalScrollIndicator = false;
			_scrollView.ShowsVerticalScrollIndicator = false;
			_scrollView.Bounces = true;
			_scrollView.PagingEnabled = true;

			_scrollView.DecelerationEnded += (sender, e) => {
				var currentPage = (int)Math.Floor ((_scrollView.ContentOffset.X - _scrollView.Frame.Width / 2) / _scrollView.Frame.Width) + 1;

				DispatchPageChange(currentPage);
			};

			AddSubview (_scrollView);

			return _scrollView;
		}

		protected void PopulateFragmentViews(List<IFragmentView> views)
		{
			if (_fragmentViews == null)
				_fragmentViews = new List<IFragmentView> ();

			foreach (var view in _fragmentViews)
				view.Cleanup ();
			
			if (views == null)
				return;

			_fragmentViews.AddRange(views);
		}

		protected void RefreshScrollViewContent()
		{
			foreach (var view in ScrollView.Subviews)
			{
				view.RemoveFromSuperview ();
				view.Dispose ();
			}

			foreach (var fragment in _fragmentViews)
			{
				if (fragment.View == null)
					fragment.Create ();

				ScrollView.AddSubview (fragment.View);
			}

			SetNeedsLayout ();

			DispatchPageChange (0);
		}

		public void DispatchPageChange(int pageIndex)
		{
			if (_fragmentViews == null)
				return;
			
			int count = 0;
			foreach (var fragment in _fragmentViews)
			{
				if (count == pageIndex)
					fragment.Show ();
				else
					fragment.Cleanup ();

				count++;
			}
		}
	}
}