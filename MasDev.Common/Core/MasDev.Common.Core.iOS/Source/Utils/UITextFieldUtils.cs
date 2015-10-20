using UIKit;

namespace MasDev.iOS.Utils
{
	public static class UITextFieldUtils
	{
		private static UITextField _stub = new UITextField();

		public static void CheckIsValid(this UITextField txtCurrent, bool clause)
		{
			if (txtCurrent == null)
				return;

			txtCurrent.BackgroundColor = clause ? _stub.BackgroundColor : UIColor.Red.ColorWithAlpha (0.5f);
			txtCurrent.Layer.CornerRadius = clause ? _stub.Layer.CornerRadius : 5f;
		}
	}
}