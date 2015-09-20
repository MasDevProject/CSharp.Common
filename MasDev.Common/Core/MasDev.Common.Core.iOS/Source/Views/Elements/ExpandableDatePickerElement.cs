using System;
using MonoTouch.Dialog;
using UIKit;
using Foundation;
using System.Linq;
using CoreGraphics;
using MasDev.iOS.Extensions;

namespace MasDev.iOS.Views.Elements
{
	public class ExpandableDatePickerElement : Element, IElementSizing
	{
		Section _parentSection;
		UITableViewCell _stringElementCell;
		DateTime _currentDate;
		UIViewElement _viewElement;
		String _caption;
		UIDatePickerMode _mode;

		public float FontSize { get; set; }

		public UIDatePicker Picker { get; set; }
		public Boolean Opened { get; set; }

		public Boolean Enabled { get; set; }
		public UIColor TextColor { get; set; }
		public UIColor SelectedTextColor { get; set; }

		private DateTime _maxDate, _minDate;

		public DateTime MaxDate
		{
			get { return _maxDate; }
			set { _maxDate = value; UpdatePicker (); }
		}
		public DateTime MinDate
		{
			get { return _minDate; }
			set { _minDate = value; UpdatePicker (); }
		}

		public event Action<DateTime> DateChanged;

		public DateTime Date
		{
			get
			{
				var date = _currentDate;

				if (Picker != null)
					date = Picker.Date.ToDateTime().ToLocalTime();

				return date;
			}
		}

		public ExpandableDatePickerElement (Section parentSection, String caption, DateTime currentDate, UIDatePickerMode mode) : this(parentSection, caption, currentDate)
		{
			_mode = mode;
		}

		public ExpandableDatePickerElement (Section parentSection, String caption, DateTime currentDate)
			:base("date")
		{
			this._currentDate = currentDate;
			_parentSection = parentSection;
			_caption = caption;

			_mode = UIDatePickerMode.DateAndTime;
			Enabled = true;
			FontSize = 14f;

			TextColor = UIColor.LightGray;
			SelectedTextColor = UIColor.LightGray;
		}

		private void InitializePicker()
		{
			Picker = new UIDatePicker (new CGRect (0, 0, 320, 200));
			Picker.Mode = _mode;
			Picker.TimeZone = NSTimeZone.LocalTimeZone;
			Picker.Locale = new NSLocale (NSLocale.CurrentLocale.LanguageCode);

			Picker.Date = _currentDate.ToNSDate();

			if(MaxDate != DateTime.MinValue)
				Picker.MaximumDate = MaxDate.ToNSDate();
			if (MinDate != DateTime.MinValue)
				Picker.MinimumDate = MinDate.ToNSDate ();

			Picker.ValueChanged += (object sender, EventArgs e) => 
			{
				_stringElementCell.DetailTextLabel.Text = FormattedStringNSDate();
				if(DateChanged != null)
					DateChanged.Invoke(Picker.Date.ToDateTime());
			};

			Picker.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
			_viewElement = new CenteredViewElement (Picker);
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			if (!Enabled)
				return;

			base.Selected (dvc, tableView, path);

			if(Picker == null)
				InitializePicker ();

			_stringElementCell.DetailTextLabel.Text = FormattedStringNSDate();

			bool isToExpand = !_parentSection.Elements.Contains (_viewElement);

			var existingElements = _parentSection.Elements.OfType<ExpandableDatePickerElement> ().ToList();

			int offset = 1;
			foreach (var element in existingElements) 
			{
				if (element.Collapse () && element.IndexPath != null && element.IndexPath.Row < path.Row)
					offset = 0;
			}

			if (isToExpand)
			{
				_stringElementCell.DetailTextLabel.TextColor = SelectedTextColor;

				_parentSection.Insert (Math.Min(path.Row + offset, _parentSection.Elements.Count ()), UITableViewRowAnimation.Automatic, _viewElement);

				Opened = true;
			}
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			const string cellKey = "ExpandableDatePickerElement";
			var cell = tv.DequeueReusableCell (cellKey);

			if (cell == null) {
				cell = new UITableViewCell (UITableViewCellStyle.Value1, cellKey);

				//cell.TextLabel.Font = UIFont.SystemFontOfSize (FontSize);
				cell.TextLabel.TextColor = UIColor.Black;
				cell.TextLabel.AdjustsFontSizeToFitWidth = true;
				cell.TextLabel.MinimumScaleFactor = .5f;
				cell.Accessory = UITableViewCellAccessory.None;
				cell.SelectionStyle = UITableViewCellSelectionStyle.None;

				//cell.DetailTextLabel.Font = UIFont.SystemFontOfSize (FontSize);
				cell.DetailTextLabel.TextColor = TextColor;
				cell.DetailTextLabel.AdjustsFontSizeToFitWidth = true;
				cell.DetailTextLabel.MinimumScaleFactor = .5f;

				cell.ImageView.Image = UIImage.FromBundle("Ic_clock.png");
			}

			cell.TextLabel.Text = _caption;
			cell.DetailTextLabel.Text = FormattedStringNSDate ();

			_stringElementCell = cell;

			return _stringElementCell;
		}

		public bool Collapse() // return true if the element was open
		{
			var res = Opened;
			Opened = false;

			_parentSection.Remove (_viewElement);
			if(_stringElementCell != null)
				_stringElementCell.DetailTextLabel.TextColor = TextColor;

			return res;
		}

		private String CurrentFormat()
		{
			var dateFormat = "dd/MMM/yy HH:mm";

			switch (_mode) 
			{
			case UIDatePickerMode.Date:
				dateFormat = "dddd, dd MMMM yyyy";
				break;
			case UIDatePickerMode.Time:
				dateFormat = "HH:mm";
				break;
			}

			return dateFormat;
		}

		private String FormattedStringNSDate()
		{
			if (Picker == null)
				return _currentDate.ToString (CurrentFormat ());

			return Picker.Date.ToDateTime().ToString (CurrentFormat());
		}

		public void AdjustTextStyle(bool wrong)
		{
			if (_stringElementCell == null)
				return;

			UIStringAttributes attr = null;

			if (wrong)
				attr = new UIStringAttributes { StrikethroughStyle = NSUnderlineStyle.Single };

			_stringElementCell.DetailTextLabel.AttributedText = new NSAttributedString(_stringElementCell.DetailTextLabel.Text,
				attr);
		}

		private void UpdatePicker()
		{
			if (Picker != null)
			{
				if(MaxDate != DateTime.MinValue)
					Picker.MaximumDate = MaxDate.ToNSDate();
				if (MinDate != DateTime.MinValue)
					Picker.MinimumDate = MinDate.ToNSDate ();
			}
		}

		public void AdjustDate (DateTime date)
		{
			if (_stringElementCell == null)
				return;

			_currentDate = date;
			if (Picker != null)
				Picker.Date = _currentDate.ToNSDate();

			_stringElementCell.DetailTextLabel.Text = FormattedStringNSDate ();
		}

		public nfloat GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			return 44;
		}
	}
}