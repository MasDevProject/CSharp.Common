using System;
using MonoTouch.Dialog;
using UIKit;
using CoreGraphics;
using Foundation;
using MasDev.iOS.Extensions;

namespace MasDev.iOS.Views.Elements
{
	public class SegmentedControlElement : Element, IElementSizing
	{
		protected const string Identifier = "SegmentedControlElement";
		protected const float RowHeight = 44f;
		protected const int DefaultTagValue = 1;
		protected const float Padding = 8f;

		public UISegmentedControl SegmentedControl { get ; private set; }

		protected nint _value;
		public nint Value
		{ 
			get
			{
				if (SegmentedControl != null)
					return SegmentedControl.SelectedSegment;
				
				return _value;
			}
			set 
			{
				_value = value;
				UpdateValue(_value);
			}
		}

		protected string[] _titles;
		public string[] Titles
		{ 
			get { return _titles; }
			set 
			{
				_titles = value;
				UpdateTitles(value);
			}
		}

		public SegmentedControlElement(string[] titles, int value) : base(string.Empty)
		{
			_value = value;
			_titles = titles;
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			var segmentedControlViewCell = tv.DequeueReusableCell (Identifier);

			if (segmentedControlViewCell == null) {
				segmentedControlViewCell = new UITableViewCell (UITableViewCellStyle.Default, Identifier);
				segmentedControlViewCell.SelectionStyle = UITableViewCellSelectionStyle.None;
			} else
				RemoveTag (segmentedControlViewCell, DefaultTagValue);

			if (SegmentedControl == null)
			{
				SegmentedControl = new UISegmentedControl(new CGRect(Padding * 2, Padding / 2, tv.Bounds.Width - (Padding * 4), segmentedControlViewCell.ContentView.Bounds.Height - Padding));
				SegmentedControl.Tag = DefaultTagValue;

				UpdateTitles (_titles);
			}

			SegmentedControl.SelectedSegment = _value;

			segmentedControlViewCell.ContentView.AddSubview (SegmentedControl);

			return segmentedControlViewCell;
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

		private void UpdateValue(nint value)
		{
			if(SegmentedControl != null)
				SegmentedControl.SelectedSegment = value;
		}

		private void UpdateTitles(string[] value)
		{
			if (SegmentedControl == null || value == null)
				return;
			
			SegmentedControl.RemoveAllSegments ();

			var count = 0;
			foreach (var title in value)
				SegmentedControl.InsertMultilineTitle(title, count++, false);
			
			SegmentedControl.SelectedSegment = _value;
		}
	}
}