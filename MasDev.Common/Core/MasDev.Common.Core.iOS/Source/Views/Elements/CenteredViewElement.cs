using System;
using MonoTouch.Dialog;
using UIKit;

namespace MasDev.iOS.Views.Elements
{
	public class CenteredViewElement : UIViewElement
	{
		public CenteredViewElement (UIView contentView) : base(String.Empty, contentView, false)
		{
		}

		public override UITableViewCell GetCell (UIKit.UITableView tv)
		{
			var cell = base.GetCell (tv);

			var center = this.View.Center;
			center.X = tv.Center.X;
			this.View.Center = center;

			return cell;
		}
	}
}