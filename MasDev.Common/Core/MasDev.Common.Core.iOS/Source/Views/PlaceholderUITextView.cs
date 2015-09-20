using System;
using UIKit;
using CoreGraphics;

namespace MasDev.iOS.Views
{
	public class PlaceholderUITextView : UITextView
	{
		private UIColor _fontColor { get; set; }
		private Boolean _shouldRemovePlaceHolder { get; set; }

		/// <summary>
		/// Gets or sets the placeholder to show prior to editing - doesn't exist on UITextView by default
		/// </summary>
		public string Placeholder { get; set; }

		public string ActualText { get { return	_shouldRemovePlaceHolder ? String.Empty : Text; } }

		public PlaceholderUITextView ()
		{
			Initialize ();
		}

		public PlaceholderUITextView (CGRect frame)
			: base(frame)
		{
			Initialize ();
		}

		public PlaceholderUITextView (IntPtr handle)
			: base(handle)
		{
			Initialize ();
		}

		private void Initialize()
		{
			_shouldRemovePlaceHolder = false;

			Placeholder = "Please enter text";

			ShouldBeginEditing = t => {
				if (_shouldRemovePlaceHolder && Text == Placeholder) {
					Text = string.Empty;

					if(_fontColor != null)
						TextColor = _fontColor;
				}
				return true;
			};

			ShouldEndEditing = t => {
				ShowPlaceholder();
				return true;
			};

			Changed += (object sender, EventArgs e) => 
			{
				if(_shouldRemovePlaceHolder)
				{
					Text = Text.Replace(Placeholder, String.Empty);
					TextColor = _fontColor;

					_shouldRemovePlaceHolder = false;
				}

				ShowPlaceholder ();
			};
		}

		private void ShowPlaceholder()
		{
			if (string.IsNullOrEmpty (Text)) {
				_fontColor = TextColor ?? UIColor.Black;
				TextColor = UIColor.LightGray;

				Text = Placeholder;

				SelectedRange = new Foundation.NSRange (0, 0);

				_shouldRemovePlaceHolder = true;
			}
		}

		public void Refresh()
		{
			ShowPlaceholder ();
		}
	}
}