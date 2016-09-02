using System;
using UIKit;
using CoreGraphics;

namespace MasDev.Common
{
	public static class UICollectionViewExtensions
	{
		const int Margin = 8;
		const int MinSpacing = 8;

		public static void EnableSelfSizingCells(this UICollectionView collectionView, CGSize estimatedSize)
		{
			var flowLayout = collectionView.CollectionViewLayout as UICollectionViewFlowLayout;
			if (flowLayout != null)
				flowLayout.EstimatedItemSize = estimatedSize;
		}

		public static void ForceEqualSpacing(this UICollectionView collectionView, CGSize itemSize, bool removeTopMargin = false)
		{
			var layout = collectionView.CollectionViewLayout as UICollectionViewFlowLayout;

			var screenWidth = collectionView.Bounds.Width;

			var cardWidth = itemSize.Width;
			var cardPerLines = (int) (screenWidth / cardWidth);

			var totalSpacing = screenWidth - (cardWidth * cardPerLines);
			if (totalSpacing < 0)
				totalSpacing = 0;

			var singleSpacing = totalSpacing / (cardPerLines + 1);

			if (singleSpacing < MinSpacing && cardPerLines > 1)
			{
				cardPerLines--;

				totalSpacing = screenWidth - (cardWidth * cardPerLines);
				if (totalSpacing < 0)
					totalSpacing = 0;

				singleSpacing = totalSpacing / (cardPerLines + 1);
			}

			layout.SectionInset = new UIEdgeInsets (removeTopMargin ? 0 : singleSpacing, singleSpacing, singleSpacing, singleSpacing);
			layout.MinimumInteritemSpacing = singleSpacing;
			layout.MinimumLineSpacing = singleSpacing;
		}

		public static void ScrollToTop(this UICollectionView collectionView, bool animated)
		{
			collectionView.SetContentOffset (CGPoint.Empty, animated);
		}
	}
}