using System;
using Android.App;

namespace MasDev.Common.Droid
{
	public static class DialogExtensions
	{
		public static bool TryDismiss (this Dialog dialog)
		{
			try {
				dialog.Dismiss ();
				return true;
			}catch(Exception) {
				return false;
			}
		}
	}
}

