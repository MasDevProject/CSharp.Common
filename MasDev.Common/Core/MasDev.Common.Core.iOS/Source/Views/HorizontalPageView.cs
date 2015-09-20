using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using System.Linq;

namespace MasDev.iOS.Views
{
	public class HorizontalPageView : UITableView
	{
		public HorizontalPageView (CGRect frame) : base(frame)
		{
			SeparatorStyle = UITableViewCellSeparatorStyle.None;
			BackgroundColor = UIColor.Clear;
			AllowsSelection = false;
		}

		public void AddViews(List<UIView> views)
		{
			Source = new HorizontalPageTableSource(HorizontalPageTableSource.ToPaddedViews(views));
		}
	}

	public class HorizontalPageTableSource : UITableViewSource
	{
		public event Action<UIScrollView> OnTableScrolled = delegate { };

		private List<UIView> items;

		public HorizontalPageTableSource(List<UIView> items)
		{
			this.items = items;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return items.Count;
		}

		public override nfloat GetHeightForRow (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			return items [indexPath.Row].Frame.Height;
		}

		public override void RowSelected (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			tableView.DeselectRow (indexPath, true);
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var item = items [indexPath.Row];

			var cell = tableView.DequeueReusableCell (HorizontalPageTableViewCell.Identifier) as HorizontalPageTableViewCell;
			if (cell == null)
				cell = new HorizontalPageTableViewCell ();

			cell.Frame = new CGRect (0, 0, tableView.Frame.Width, item.Frame.Height);

			cell.BindData (item);

			return cell;
		}

		public override void Scrolled (UIScrollView scrollView)
		{
			if (OnTableScrolled != null)
				OnTableScrolled.Invoke (scrollView);
		}

		public static List<UIView> ToPaddedViews(List<UIView> views)
		{
			var convertedViews = new List<UIView>();
			foreach(var view in views)
				convertedViews.Add(PaddedView.FromView(view));

			return convertedViews;
		}
	}

	class PaddedView : UIView
	{
		protected const float padding = 8;

		public PaddedView(CGRect frame)
		{
			var baseFrame = frame;
			baseFrame.Width += padding * 2;
			baseFrame.Height += padding * 2;

			Frame = baseFrame;
		}

		public override void AddSubview (UIView view)
		{
			view.Frame = new CGRect (padding, padding, view.Frame.Width, view.Frame.Height);

			base.AddSubview (view);
		}

		public static UIView FromView(UIView view)
		{
			var paddedView = new PaddedView (view.Bounds);

			paddedView.BackgroundColor = view.BackgroundColor;
			view.BackgroundColor = UIColor.Clear;

			paddedView.AddSubview (view);

			return paddedView;
		}
	}

	class HorizontalPageTableViewCell : UITableViewCell
	{
		public const string Identifier = "HorizontalPageTableViewCell";

		public HorizontalPageTableViewCell() : base(UITableViewCellStyle.Default, Identifier)
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
		}

		public void BindData(UIView view)
		{
			Subviews.OfType<PaddedView>().ToList().ForEach(v => v.RemoveFromSuperview());

			view.RemoveFromSuperview ();

			view.Center = Center;

			AddSubview (view);
		}
	}
}