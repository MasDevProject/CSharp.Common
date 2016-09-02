using System;
using UIKit;

namespace MasDev.Common
{
	public static class UIAlertControllerUtils
	{
		public static void ShowGenericAlert (UIViewController presenter, string title, string message, string actionTitle,
			Action<UIAlertAction> callback = null, UIAlertControllerStyle style = UIAlertControllerStyle.Alert)
		{
			var alert = UIAlertController.Create (title, message, style);

			alert.AddAction (UIAlertAction.Create (actionTitle, UIAlertActionStyle.Default, callback));

			presenter.PresentViewController (alert, true, null);
		}
	}
}