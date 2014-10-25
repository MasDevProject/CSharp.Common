﻿using Android.Widget;
using Android.Graphics.Drawables;


namespace MasDev.Common.Droid.ExtensionMethods
{
	public static class TextViewExtensions
	{
		public static void ShowError(this TextView textView, string message, Drawable errorIcon = null)
		{
			textView.RequestFocus ();
			textView.Focusable = true;
			textView.SetError (message, errorIcon ?? textView.Context.Resources.GetDrawable (Resource.Drawable.Ic_textview_error));
		}
	}
}

