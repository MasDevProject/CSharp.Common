using System;
using MonoTouch.Dialog;
using UIKit;
using Foundation;
using CoreGraphics;

namespace MasDev.iOS.Views.Elements
{
	public class RatingElement : Element, IElementSizing
	{
		protected const string Identifier = "RatingTableViewCell";
		protected const float RowHeight = 44f;
		protected const int DefaultTagValue = 1;

		public RatingView RatingView { get ; private set; }

		protected float _value;

		protected UIImage imgEmpty;
		protected UIImage imgHalf;
		protected UIImage imgFull;

		public float Value
		{ 
			get { return _value; }
			set 
			{
				_value = value;
				UpdateValue(_value);
			}
		}

		public RatingElement(UIImage imgEmpty, UIImage imgHalf, UIImage imgFull) : base(string.Empty)
		{
			this.imgEmpty = imgEmpty;
			this.imgHalf = imgHalf;
			this.imgFull = imgFull;

			_value = 0f;
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			var ratingCellView = tv.DequeueReusableCell (Identifier);

			if (ratingCellView == null) {
				ratingCellView = new UITableViewCell (UITableViewCellStyle.Default, Identifier);
				ratingCellView.SelectionStyle = UITableViewCellSelectionStyle.None;
			} else
				RemoveTag (ratingCellView, DefaultTagValue);

			if (RatingView == null)
			{
				RatingView = new RatingView (
					new CGRect(0, 0, tv.Bounds.Width, ratingCellView.ContentView.Bounds.Height),
					imgEmpty,
					imgHalf,
					imgFull);
				
				RatingView.Tag = DefaultTagValue;
				RatingView.OnRatingChanged += (newRating) => { Value = newRating; };
			}

			RatingView.Rating = _value;

			ratingCellView.ContentView.AddSubview (RatingView);

			return ratingCellView;
		}

		public nfloat GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			return RowHeight;
		}

		protected new void RemoveTag (UITableViewCell cell, int tag)
		{
			var viewToRemove = cell.ContentView.ViewWithTag (tag);
			if (viewToRemove != null)
				viewToRemove.RemoveFromSuperview ();
		}

		private void UpdateValue(float value)
		{
			if(RatingView != null)
				RatingView.Rating = value;
		}
	}
}