using System;
using MonoTouch.Dialog;
using UIKit;

namespace MasDev.iOS.Views.Elements
{
	public class StyleableStringElement : StringElement
	{
		readonly UIColor _textColor;

		public StyleableStringElement (string caption) : base(caption)
		{
		}

		public StyleableStringElement(string caption, string value) : base(caption, value)
		{
		}

		public StyleableStringElement(string caption, Action tapAction, UIColor textColor) : base(caption, tapAction)
		{
			_textColor = textColor;

			Alignment = UITextAlignment.Center;
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			var styleableStringCell =  base.GetCell (tv);

			styleableStringCell.TextLabel.TextColor = _textColor;

			return styleableStringCell;
		}
	}
}