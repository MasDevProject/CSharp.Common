using UIKit;
using CoreGraphics;

namespace MasDev.iOS.Views
{
	public class EmptyTableSourceView : UIView
	{
		private const float Padding = 16;

		private string imagePath;
		private string message;

		public string ImagePath
		{
			get
			{
				return imagePath;
			}
			set
			{
				imagePath = value;
				Initialize ();
			}
		}

		public string MessageText
		{
			get { return message; }
			set
			{
				message = value;
				Initialize ();
			}
		}

		public EmptyTableSourceView (CGRect frame) : base(frame)
		{
		}

		private void Initialize()
		{
			UIImage image = null;
			if (!string.IsNullOrWhiteSpace (imagePath))
				image = UIImage.FromFile (imagePath);

			// Init ImageView

			UIImageView imageView = null;
			if (image != null)
			{
				imageView = new UIImageView (image);

				var imgFrame = imageView.Frame;
				imgFrame.Location = new CGPoint(Center.X - (imgFrame.Width / 2), Center.Y - (imgFrame.Height / 2));
				imageView.Frame = imgFrame;
			}

			// Init Label

			UILabel lblMessage = null;

			if(!string.IsNullOrEmpty(message))
				lblMessage = new UILabel (new CGRect(0, 0, Frame.Width - (Padding * 2), 44f));

			if (lblMessage != null) {
				lblMessage.Lines = 0;
				lblMessage.LineBreakMode = UILineBreakMode.WordWrap;
				lblMessage.TextAlignment = UITextAlignment.Center;

				lblMessage.Text = message;
				lblMessage.TextColor = UIColor.LightGray;
				lblMessage.SizeToFit ();

				lblMessage.Center = Center;
				if (imageView != null) {
					var lblFrame = lblMessage.Frame;
					lblFrame.Location = new CGPoint (lblMessage.Frame.X, imageView.Frame.Y + imageView.Frame.Height + Padding);
					lblMessage.Frame = lblFrame;
				}
			}

			if (imageView != null)
				AddSubview (imageView);
			if (lblMessage != null)
				AddSubview (lblMessage);
		}
	}
}