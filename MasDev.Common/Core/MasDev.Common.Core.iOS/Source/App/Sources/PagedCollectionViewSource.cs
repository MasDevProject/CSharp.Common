using System;
using MasDev.Collections;
using UIKit;
using Foundation;
using MasDev.Utils;
using System.Linq;

namespace MasDev.Common
{
	public abstract class PagedCollectionViewSource<T> : BaseCollectionViewSource<T>
	{
		public event Action<T> OnDataLoaded;

		protected IPagedEnumerable<T> PagedEnumerable;

		protected bool HasMorePage { get { return PagedEnumerable == null || PagedEnumerable.HasMorePages; } }

		protected PagedCollectionViewSource(IPagedEnumerable<T> pagedEnumerable) : base(pagedEnumerable.Items)
		{
			PagedEnumerable = pagedEnumerable;
		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
		{
			if (RequestNextPage(indexPath))
				LoadNextPage (collectionView);

			return base.GetCell (collectionView, indexPath);
		}

		// Method utils

		public async void LoadNextPage (UICollectionView collectionView)
		{
			if (!HasMorePage || PagedEnumerable == null)
				return;

			var startIndex = Items.Count;
			var count = 0;
			try
			{
				var firstPage = PagedEnumerable.CurrentPage == 0;

				var elements = await PagedEnumerable.GetNextPageAsync ();
				count = elements.Count();

				if(OnDataLoaded != null && firstPage && !CollectionUtils.IsNullOrEmpty(PagedEnumerable.Items))
					OnDataLoaded.Invoke(PagedEnumerable.Items[0]);
			}
			catch { }

			if (count == 0) 
			{
				collectionView.ReloadData ();

				return;
			}

			var indexes = new NSIndexPath[count];
			for (var i = 0; i < count; i++)
				indexes [i] = NSIndexPath.FromRowSection (i + startIndex, 0);

			collectionView.InsertItems (indexes);
		}

		public virtual void Reset()
		{
			if (PagedEnumerable != null)
				PagedEnumerable.Reset ();
		}

		protected virtual bool RequestNextPage(NSIndexPath indexPath)
		{
			return (indexPath.Row == Items.Count - 1) && HasMorePage;
		}
	}
}