using System;
using CoreGraphics;
using UIKit;

namespace MasDev.iOS.Views.States
{
	public class BaseErrorStateView : BaseStateView
	{
		const float Spacing = 16f;

		UIImageView _errorImageView;
		UILabel _errorLabel;

		UITapGestureRecognizer _gestureRecognizer;

		public BaseErrorStateView (CGRect frame, UIImage errorImage, string errorMessage) : base(frame)
		{
			if(errorImage != null)
				_errorImageView = new UIImageView (errorImage);

			if (!string.IsNullOrWhiteSpace (errorMessage)) 
			{
				_errorLabel = new UILabel ();
				_errorLabel.Lines = 0;
				_errorLabel.LineBreakMode = UILineBreakMode.WordWrap;
				_errorLabel.Text = errorMessage;
				_errorLabel.TextColor = UIColor.LightGray;
				_errorLabel.SizeToFit ();
				_errorLabel.TextAlignment = UITextAlignment.Center;
			}

			_gestureRecognizer = new UITapGestureRecognizer (ReloadAction);
			AddGestureRecognizer (_gestureRecognizer);
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			if (_errorImageView != null)
			{
				_errorImageView.RemoveFromSuperview ();

				_errorImageView.Center = Center;

				AddSubview (_errorImageView);
			}

			if (_errorLabel != null)
			{
				_errorLabel.RemoveFromSuperview ();
				_errorLabel.Frame = new CGRect (0, 0, Bounds.Width - (Spacing * 2), _errorLabel.Bounds.Height);
				_errorLabel.SizeToFit ();

				_errorLabel.Center = new CGPoint (Center.X, CenterYFromErrorImage());

				AddSubview (_errorLabel);
			}
		}

		nfloat CenterYFromErrorImage()
		{
			if (_errorImageView == null)
				return Center.Y;

			return _errorImageView.Center.Y + (_errorImageView.Bounds.Width / 2) + Spacing;
		}

		void ReloadAction()
		{
			//TODO: handle reload on error state
			Console.WriteLine ("Reload");
		}
	}
}