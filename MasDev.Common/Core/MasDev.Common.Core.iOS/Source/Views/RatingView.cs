using System;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;

namespace MasDev.iOS.Views
{
	public class RatingView : UIView
	{
		public event Action<float> OnRatingChanged = delegate { };

		public float Rating
		{
			get
			{
				return _rating;
			}
			set
			{
				if(value >= 0 && value <= MaxRating)
					_rating = value;
				
				Refresh ();
			}
		}

		public bool Editable { get; set; }

		public int MaxRating
		{
			get 
			{
				return _maxRating;
			}
			set
			{
				_maxRating = value;
				ManageStars ();
			}
		}

		protected int _maxRating;
		protected float _rating;
		protected float Margin;

		protected CGSize MinImageSize;

		protected UIImage EmptyStarImage;
		protected UIImage HalfSelectedStarImage;
		protected UIImage SelectedStarImage;

		protected List<UIImageView> ImageViews;

		protected const float MinSize = 10f;

		public RatingView (CGRect frame, UIImage imgEmpty, UIImage imgHalf, UIImage imgFull) : base(frame)
		{
			Initialize (imgEmpty, imgHalf, imgFull);
		}

		public RatingView (CGRect frame, UIImage imgEmpty, UIImage imgHalf, UIImage imgFull, float margin) 
			: this(frame, imgEmpty, imgHalf, imgFull)
		{
			Margin = margin;
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			if (EmptyStarImage == null)
				return;

			var imageCount = ImageViews.Count;
			if (imageCount < 0)
				imageCount = 0;
			
			var desiredImageWidth = (Frame.Size.Width - (Margin * 2) - imageCount) / imageCount;
			var imageWidth = (float) Math.Max (MinImageSize.Width, desiredImageWidth);
			var imageHeight = (float) Math.Max (MinImageSize.Height, Frame.Height - Margin / 2);

			imageWidth = imageHeight = Math.Min (imageWidth, imageHeight); 

			var imageSpace = (float) (Bounds.Width - (Margin * 2)) / ImageViews.Count;
			var currentX = Margin + (imageSpace - imageWidth) / 2;;

			for (int i = 0; i < ImageViews.Count; i++)
			{
				if(i > 0)
					currentX += imageSpace;

				ImageViews [i].Frame = new CGRect (
					currentX,
					Frame.Height / 2 - imageHeight / 2,
					imageWidth,
					imageHeight);
			}
		}

		public void ManageStars()
		{
			// Remove old image views
			for(int i = 0; i < ImageViews.Count; i++)
				ImageViews [i].RemoveFromSuperview ();
			
			ImageViews.Clear ();

			// Add new image views
			UIImageView imageView;
			for(int i = 0; i < _maxRating; i++)
			{
				imageView = new UIImageView();
				imageView.ContentMode = UIViewContentMode.ScaleAspectFit;

				ImageViews.Add (imageView);
				AddSubview(imageView);
			}

			// Relayout and refresh
			SetNeedsLayout();

			Refresh ();
		}

		protected void Initialize(UIImage imgEmpty, UIImage imgHalf, UIImage imgFull)
		{
			Editable = true;

			Margin = 16;

			EmptyStarImage = imgEmpty;
			HalfSelectedStarImage = imgHalf;
			SelectedStarImage = imgFull;

			ImageViews = new List<UIImageView> ();

			MinImageSize = new CGSize (MinSize, MinSize);

			MaxRating = 5;
			Rating = 0;
		}

		protected void Refresh()
		{
			UIImageView imageView;
			for(int i = 0; i < ImageViews.Count; i++)
			{
				imageView = ImageViews [i];
				if (Rating >= i + 1)
					imageView.Image = SelectedStarImage;
				else if (Rating > i)
					imageView.Image = HalfSelectedStarImage;
				else
					imageView.Image = EmptyStarImage;
			}
		}

		// Touch management

		protected void HandleTouchAtLocation (CGPoint touchLocation)
		{
			if (!Editable) return;

			var newRating = 0f;
			UIImageView imageView;
			for(int i = ImageViews.Count - 1; i >= 0; i--)
			{
				imageView = ImageViews[i];

				if (touchLocation.X > imageView.Frame.X)
				{
					newRating = i+1;
					if (touchLocation.X <= (imageView.Frame.X + imageView.Frame.Width / 2))
						newRating -= 0.5f;
					
					break;
				}
			}

			Rating = newRating;
		}

		private void HandleTouchFromNSSet(NSSet touches)
		{
			var touch = touches.AnyObject as UITouch;

			if (touch == null)
				return;

			HandleTouchAtLocation (touch.LocationInView (this));
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);

			HandleTouchFromNSSet (touches);
		}

		public override void TouchesMoved (NSSet touches, UIEvent evt)
		{
			base.TouchesMoved (touches, evt);

			HandleTouchFromNSSet (touches);
		}

		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);

			if (OnRatingChanged != null)
				OnRatingChanged.Invoke (Rating);
		}
	}
}