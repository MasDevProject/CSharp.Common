using System;
using UIKit;
using System.Collections.Generic;
using Foundation;
using System.Linq;
using MasDev.iOS.Views;

namespace MasDev.iOS.App.Sources
{
	public abstract class BaseTableViewSource<T> : UITableViewSource
	{
		public event Action<T> OnItemSelected = delegate {};

		public bool HandleEmptyState { get; set; }

		protected List<T> Items;
		protected EmptyStateSourceView EmptyView;

		protected bool FirstTime;
		protected UITableViewCellSeparatorStyle DefaultSeparatorStyle;

		protected BaseTableViewSource ()
		{
			Items = new List<T> ();

			FirstTime = true;

			HandleEmptyState = true;
		}

		protected BaseTableViewSource (List<T> items)
		{
			Items = items;

			FirstTime = true;

			HandleEmptyState = true;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			var rows = Items.Count;

			ManageEmptyTableView (tableview, rows == 0);

			return rows;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			if(OnItemSelected != null)
				OnItemSelected.Invoke(SelectedItem(indexPath));

			tableView.DeselectRow(indexPath, true);
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			return GetItemCell (tableView, SelectedItem (indexPath)); 
		}

		// Abstract methods

		protected abstract UITableViewCell GetItemCell (UITableView tableView, T item);

		// Virtual methods

		protected virtual string EmptyViewImagePath { get { return null; } }

		protected virtual string EmptyViewMessageText { get { return null; } }

		protected virtual void ManageEmptyTableView (UITableView tableView, bool isEmpty)
		{
			if (FirstTime) 
			{
				DefaultSeparatorStyle = tableView.SeparatorStyle;
				FirstTime = false;
			}

			if (!HandleEmptyState)
				return;

			if (isEmpty) {
				EmptyView = new EmptyStateSourceView (tableView.Bounds);

				EmptyView.ImagePath = EmptyViewImagePath;
				EmptyView.Message = EmptyViewMessageText;

				tableView.BackgroundView = EmptyView;

				tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			} else {
				tableView.BackgroundView = null;
				tableView.SeparatorStyle = DefaultSeparatorStyle;
			}
		}

		// Method utils

		protected T SelectedItem(NSIndexPath indexPath)
		{
			return Items != null && Items.Count > indexPath.Row ? Items.ElementAt (indexPath.Row) : default(T);
		}
	}
}