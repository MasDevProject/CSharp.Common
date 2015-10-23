using System;
using MonoTouch.Dialog;
using UIKit;
using Foundation;

namespace MasDev.iOS.Views.Elements
{
	public class CustomStringElement : Element, IElementSizing
	{
		public event Action OnTap = delegate { };

		protected const string Identifier = "CustomStringElement";
		protected float RowHeight = 44f;

		private string _caption;
		private string _value;

		private UILabel _label;
		private UITableViewCellStyle _cellStyle;
		private string _imagePath;
		private UITableViewCellAccessory _accessory;

		public string Value
		{
			get { return _value; }
			set 
			{
				_value = value;

				if (_label != null)
					_label.Text = _value;
			}
		}

		public CustomStringElement(string caption, string value, string imagePath = null) : base(caption)
		{
			_caption = caption;
			_value = value;

			_cellStyle = UITableViewCellStyle.Subtitle;
			_accessory = UITableViewCellAccessory.DisclosureIndicator;

			_imagePath = imagePath;
		}

		public CustomStringElement(string caption, string value, UITableViewCellStyle cellStyle) : this(caption, value)
		{
			_cellStyle = cellStyle;
		}

		public CustomStringElement(string caption, string value, UITableViewCellStyle cellStyle, string imagePath) : this(caption, value, cellStyle)
		{
			_imagePath = imagePath;
		}

		public CustomStringElement(string caption, string value, UITableViewCellStyle cellStyle, string imagePath, UITableViewCellAccessory accessory) : this(caption, value, cellStyle, imagePath)
		{
			_accessory = accessory;
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			base.Selected (dvc, tableView, path);

			if (OnTap != null)
				OnTap.Invoke ();

			tableView.DeselectRow(path, true);
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			var cellId = IdentifierFromStyle();

			var stringElementViewCell = tv.DequeueReusableCell (cellId);

			if (stringElementViewCell == null)
				stringElementViewCell = new UITableViewCell (_cellStyle, cellId);

			stringElementViewCell.Accessory = _accessory;

			stringElementViewCell.TextLabel.Text = _caption;
			_label = stringElementViewCell.TextLabel;

			if (stringElementViewCell.DetailTextLabel != null)
			{
				stringElementViewCell.DetailTextLabel.TextColor = UIColor.LightGray;
				stringElementViewCell.DetailTextLabel.Text = _value;
				_label = stringElementViewCell.DetailTextLabel;
			}

			if (!string.IsNullOrWhiteSpace (_imagePath) && stringElementViewCell.ImageView != null)
				stringElementViewCell.ImageView.Image = UIImage.FromBundle (_imagePath);

			return stringElementViewCell;
		}

		public nfloat GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			return RowHeight;
		}

		protected string IdentifierFromStyle()
		{
			return Identifier + _cellStyle;
		}

		public override bool Matches (string text)
		{
			return (_value != null && _value.IndexOf (text, StringComparison.CurrentCultureIgnoreCase) != -1) ||
			(_caption != null && _caption.IndexOf (text, StringComparison.CurrentCultureIgnoreCase) != -1);
		}
	}
}