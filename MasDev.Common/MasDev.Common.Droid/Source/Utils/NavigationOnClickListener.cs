using System;
using Android.Views;

namespace MasDev.Common.Droid
{
	public class NavigationOnClickListener : Java.Lang.Object, View.IOnClickListener
	{
		readonly Action _onClick;

		public NavigationOnClickListener(Action onclick)
		{
			_onClick = onclick;
		}

		public void OnClick (View v)
		{
			if (_onClick != null)
				_onClick.Invoke ();
		}
	}
}

