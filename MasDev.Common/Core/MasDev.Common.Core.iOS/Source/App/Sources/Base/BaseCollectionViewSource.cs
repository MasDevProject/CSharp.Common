using System;
using UIKit;
using System.Collections.Generic;
using Foundation;
using System.Linq;

namespace MasDev.Common
{
	public abstract class BaseCollectionViewSource<T> : UICollectionViewSource
	{
		public event Action<T> OnItemSelected = delegate {};

		protected List<T> Items;

		protected BaseCollectionViewSource ()
		{
			Items = new List<T> ();
		}

		protected BaseCollectionViewSource (List<T> items)
		{
			Items = items;
		}

		public override nint GetItemsCount (UICollectionView collectionView, nint section)
		{
			return Items != null ? Items.Count : 0;
		}

		public override void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath)
		{
			if(OnItemSelected != null)
				OnItemSelected.Invoke(SelectedItem(indexPath));

			collectionView.DeselectItem(indexPath, true);
		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
		{
			return GetItemCell (collectionView, indexPath, SelectedItem (indexPath)); 
		}

		public override bool ShouldSelectItem (UICollectionView collectionView, NSIndexPath indexPath)
		{
			return true;
		}

		// Abstract methods

		protected abstract UICollectionViewCell GetItemCell (UICollectionView collectionView, NSIndexPath indexPath, T item);

		// Method utils

		protected virtual T SelectedItem(NSIndexPath indexPath)
		{
			return Items != null && Items.Count > indexPath.Row ? Items.ElementAt (indexPath.Row) : default(T);
		}
	}
}