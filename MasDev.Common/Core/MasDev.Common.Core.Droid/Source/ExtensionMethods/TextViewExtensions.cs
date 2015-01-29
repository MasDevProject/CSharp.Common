using Android.Widget;
using Android.Graphics.Drawables;
using MasDev.Common;
using Android.Text;
using Android.Text.Style;

namespace MasDev.Droid.ExtensionMethods
{
	public static class TextViewExtensions
	{
		public static void ShowError (this TextView textView, string message, Drawable errorIcon = null)
		{
			textView.RequestFocus ();
			textView.Focusable = true;
			textView.SetError (message, errorIcon ?? textView.Context.Resources.GetDrawable (Resource.Drawable.Ic_textview_error));
		}

		public static void SetUnderlinedText (this TextView textView, string text)
		{
			var content = new SpannableString(textView);
			content.SetSpan(new UnderlineSpan(), 0, content.Length(), 0);
			textView.SetText (content, null);
		}
	}
}

