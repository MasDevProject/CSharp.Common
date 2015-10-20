using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using System.Linq;
using MasDev.Collections;

namespace MasDev.iOS.App.Sources
{
	public abstract class PagedTableViewSource<T> : BaseTableViewSource<T>
	{
		protected bool HasMorePage;

		UITableViewCell loadMoreTableViewCell;

		protected IPagedEnumerable<T> PagedEnumerable;

		protected PagedTableViewSource(IPagedEnumerable<T> pagedEnumerable)
		{
			PagedEnumerable = pagedEnumerable;

			HasMorePage = true;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return !HasMorePage ? 
				base.RowsInSection(tableview, section) :
				Items.Count + 1;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			if (RequestNextPage(indexPath))
			{
				if (loadMoreTableViewCell == null)
				{
					loadMoreTableViewCell = new LoadMoreTableViewCell ();
					((LoadMoreTableViewCell) loadMoreTableViewCell).Initialize ();

					if (FirstTime)
					{
						DefaultSeparatorStyle = tableView.SeparatorStyle;
						FirstTime = false;
					}

					tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
				}

				LoadNextPage (tableView);

				return loadMoreTableViewCell;
			}

			return base.GetCell (tableView, indexPath);
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return !RequestNextPage (indexPath) ? GetItemCellRowHeight(tableView, indexPath) : LoadMoreTableViewCell.RowHeight;
		}

		// Abstract methods

		protected abstract nfloat GetItemCellRowHeight(UITableView tableView, NSIndexPath indexPath);

		// Method utils

		public async void LoadNextPage (UITableView tableView)
		{
			if (!HasMorePage || PagedEnumerable == null)
				return;
			try
			{
				var newItems = await PagedEnumerable.GetNextPageAsync ();
				if (newItems != null)
					Items.AddRange (newItems);

				HasMorePage = PagedEnumerable.HasMorePages;	
			}
			catch
			{
				HasMorePage = false;
				//TODO: manage network error
			}

			if (Items.Any ())
				tableView.SeparatorStyle = DefaultSeparatorStyle;

			tableView.ReloadData ();
		}

		public void Reset()
		{
			if (PagedEnumerable != null)
				PagedEnumerable.Reset ();

			if (Items != null)
				Items.Clear ();

			HasMorePage = PagedEnumerable.HasMorePages;
		}

		protected bool RequestNextPage(NSIndexPath indexPath)
		{
			return indexPath.Row == Items.Count && HasMorePage;
		}
	}

	public class LoadMoreTableViewCell : UITableViewCell
	{
		public const string Identifier = "LoadMoreTableViewCell";

		public const float RowHeight = 44f;

		private UIActivityIndicatorView activityIndicatorView;

		public LoadMoreTableViewCell() : base(UITableViewCellStyle.Default, Identifier)
		{
			SelectionStyle = UITableViewCellSelectionStyle.None;
		}

		public void Initialize()
		{
			activityIndicatorView = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.Gray);
			activityIndicatorView.StartAnimating ();

			ContentView.AddSubview (activityIndicatorView);
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			if(activityIndicatorView != null)
				activityIndicatorView.Center = ContentView.Center;
		}
	}
}