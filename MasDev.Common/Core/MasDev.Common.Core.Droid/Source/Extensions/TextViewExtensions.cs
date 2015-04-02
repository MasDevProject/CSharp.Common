using Android.Widget;
using Android.Graphics.Drawables;
using MasDev.Common;
using Android.Text;
using Android.Text.Style;
using System;
using Android.Views.InputMethods;

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

		public static void ShowError (this TextView textView, int messageId, Drawable errorIcon = null)
		{
			textView.RequestFocus ();
			textView.Focusable = true;
			textView.SetError (textView.Context.GetString (messageId), errorIcon ?? textView.Context.Resources.GetDrawable (Resource.Drawable.Ic_textview_error));
		}

		public static void SetUnderlinedText (this TextView textView, string text)
		{
			var content = new SpannableString(text);
			content.SetSpan(new UnderlineSpan(), 0, content.Length(), 0);
			textView.SetText (content, null);
		}

		public static void AddOnEnterPressedListener (this TextView textView, Action action)
		{
			textView.SetOnEditorActionListener (new OnEditorActionListener (action));
		}

		class OnEditorActionListener : Java.Lang.Object, TextView.IOnEditorActionListener
		{
			readonly Action _action;
			public OnEditorActionListener (Action action)
			{
				_action = action;
			}

			public bool OnEditorAction (TextView v, ImeAction actionId, Android.Views.KeyEvent e)
			{
				if (actionId == ImeAction.Done)
					_action.Invoke ();

				return false;
			}
		}
	}
}

