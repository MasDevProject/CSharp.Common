using System;
using UIKit;
using Foundation;
using MasDev.Collections;
using MasDev.Utils;

namespace MasDev.iOS.App.Sources
{
	public abstract class PagedTableViewSource<T> : BaseTableViewSource<T>
	{
		public event Action<T> OnDataLoaded;

		protected BasePagedEnumerable<T> PagedEnumerable;

		protected bool HasMorePage { get { return PagedEnumerable == null || PagedEnumerable.HasMorePages; } }

		UITableViewCell loadMoreTableViewCell;

		protected PagedTableViewSource(BasePagedEnumerable<T> pagedEnumerable) : base(pagedEnumerable.Items)
		{
			PagedEnumerable = pagedEnumerable;
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
				var firstPage = PagedEnumerable.CurrentPage == 0;

				await PagedEnumerable.GetNextPageAsync ();

				if(OnDataLoaded != null && firstPage && !CollectionUtils.IsNullOrEmpty(PagedEnumerable.Items))
					OnDataLoaded.Invoke(PagedEnumerable.Items[0]);
			}
			catch { }

			tableView.ReloadData ();
		}

		public virtual void Reset()
		{
			if (PagedEnumerable != null)
				PagedEnumerable.Reset ();
		}

		protected virtual bool RequestNextPage(NSIndexPath indexPath)
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