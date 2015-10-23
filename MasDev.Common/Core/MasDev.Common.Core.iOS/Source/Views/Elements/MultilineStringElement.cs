using System;
using MonoTouch.Dialog;
using UIKit;
using Foundation;
using CoreGraphics;

namespace MasDev.iOS.Views.Elements
{
	public class MultilineStringElement : Element, IElementSizing
	{
		protected const string Identifier = "MultilineStringElement";
		protected const int DefaultTagValue = 1;
		protected const float Padding = 16f;

		public UILabel TextLabel { get ; private set; }

		protected string _value;
		public string Value
		{ 
			get { return _value; }
			set 
			{
				_value = value;
				UpdateValue(_value);
			}
		}

		protected float RowHeight = 44f;

		private UILabel _stubLabel = new UILabel ();

		public MultilineStringElement(string value) : base(string.Empty)
		{
			_value = value;
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			var labelViewCell = tv.DequeueReusableCell (Identifier);

			if (labelViewCell == null) {
				labelViewCell = new UITableViewCell (UITableViewCellStyle.Default, Identifier);
				labelViewCell.SelectionStyle = UITableViewCellSelectionStyle.None;
			} else
				RemoveTag (labelViewCell, DefaultTagValue);

			CreateTextLabel (tv.Bounds);

			labelViewCell.ContentView.AddSubview (TextLabel);

			return labelViewCell;
		}

		public nfloat GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			return Math.Max(
				RowHeight,
				(float)_value.StringSize(
					_stubLabel.Font,
					new CGSize(tableView.Bounds.Width - Padding * 2, float.MaxValue),
					UILineBreakMode.WordWrap).Height + Padding * 2);
		}

		void CreateTextLabel(CGRect frame)
		{
			if (TextLabel == null)
			{
				TextLabel = new UILabel (
					new CGRect (Padding, Padding, frame.Width - Padding * 2, frame.Height - Padding * 2));
				TextLabel.Tag = DefaultTagValue;

				TextLabel.Lines = 0;
				TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
			}

			TextLabel.Text = _value;
			TextLabel.SizeToFit ();
		}

		protected new void RemoveTag (UITableViewCell cell, int tag)
		{
			var viewToRemove = cell.ContentView.ViewWithTag (tag);
			if (viewToRemove != null)
				viewToRemove.RemoveFromSuperview ();
		}

		private void UpdateValue(string value)
		{
			if (TextLabel != null)
			{
				TextLabel.Text = value;
				TextLabel.SizeToFit ();
			}
		}
	}
}