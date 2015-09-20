using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;

namespace MasDev.iOS.Views
{
	public class PickerView<T> : UIView
	{
		UIPickerView _picker;
		StatusPickerViewModel<T> _pickerModel;
		UITapGestureRecognizer _tapRecognizer;
		bool _pickerVisible;

		Action<T> _itemSelectedAction;

		public bool Visible { get { return _pickerVisible; } }

		public PickerView (CGRect frame, List<T> items, Action<T> itemSelectedAction)
		{
			Frame = frame;

			AutoresizingMask = UIViewAutoresizing.FlexibleWidth |
							UIViewAutoresizing.FlexibleHeight |
							UIViewAutoresizing.FlexibleBottomMargin;

			_picker = new UIPickerView ();
			_picker.BackgroundColor = UIColor.Black.ColorWithAlpha(0.7f);
			_pickerModel = new StatusPickerViewModel<T> (items);
			_picker.Model = _pickerModel;

			BackgroundColor = UIColor.Clear;

			_itemSelectedAction = itemSelectedAction;

			AddTapGestureRecognizer ();
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			if (_picker != null)
				_picker.Center = new CGPoint (Center.X, _picker.Center.Y);
		}

		void AddTapGestureRecognizer()
		{
			if (_tapRecognizer != null)
				return;

			_tapRecognizer = new UITapGestureRecognizer ();

			_tapRecognizer.AddTarget (() => 
				{
					if(_pickerVisible)
					{
						var category = TogglePicker(this.Superview);
						if(category != null && _itemSelectedAction != null)
							_itemSelectedAction.Invoke(category);
					}
				});

			AddGestureRecognizer (_tapRecognizer);
			_tapRecognizer.CancelsTouchesInView = false;
		}

		public T TogglePicker(UIView containerView, CGRect frame)
		{
			if (frame != CGRect.Empty)
			{
				Frame = frame;
				_picker.Center = new CGPoint (Center.X, _picker.Center.Y);
			}

			return TogglePicker (containerView);
		}

		public T TogglePicker(UIView containerView)
		{
			var result = default(T);

			var initialFrame = _picker.Frame;

			if (!_pickerVisible)
			{
				var pFrame = _picker.Frame;
				pFrame.Y = -pFrame.Height;
				_picker.Frame = pFrame;

				UIView.Animate (0.3, () => {
					_picker.Frame = initialFrame;
				});

				AddSubview (_picker);

				if(containerView != null)
					containerView.AddSubview (this);
			}
			else
			{
				var pFrame = _picker.Frame;
				pFrame.Y = -pFrame.Height;

				UIView.Animate (0.3, () => {
						_picker.Frame = pFrame;
					}, () => {
						_picker.RemoveFromSuperview ();
						_picker.Frame = initialFrame;
						RemoveFromSuperview ();
					});
			}

			if(_pickerVisible)
				result =_pickerModel.SelectedItem(_picker);

			_pickerVisible = !_pickerVisible;

			return result;
		}
	}

	class StatusPickerViewModel<T> : UIPickerViewModel
	{
		readonly List<T> _items;

		public StatusPickerViewModel(List<T> items)
		{
			_items = items;
		}

		public override nint GetComponentCount (UIPickerView pickerView)
		{
			return 1;
		}

		public override nint GetRowsInComponent (UIPickerView pickerView, nint component)
		{
			return _items.Count;
		}

		public override string GetTitle (UIPickerView pickerView, nint row, nint component)
		{
			return _items[(int)row].ToString();
		}

		public override Foundation.NSAttributedString GetAttributedTitle (UIPickerView pickerView, nint row, nint component)
		{
			return new Foundation.NSAttributedString (_items[(int)row].ToString(), UIFont.SystemFontOfSize(UIFont.SystemFontSize), UIColor.White);
		}

		public T SelectedItem(UIPickerView pickerView)
		{
			return _items [(int)pickerView.SelectedRowInComponent (0)];
		}
	}
}