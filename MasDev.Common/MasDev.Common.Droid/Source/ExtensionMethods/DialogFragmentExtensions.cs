using Android.Support.V4.App;
using System;

namespace MasDev.Droid.ExtensionMethods
{
	public static class DialogFragmentExtensions
	{
		public static void ShowIfNotAlreadyShowed (this DialogFragment dialog, FragmentManager fragmentManager, string tag)
		{
			var prev = fragmentManager.FindFragmentByTag (tag);
			if (prev != null)
				return;

			var ft = fragmentManager.BeginTransaction ().AddToBackStack (null);
			dialog.Show (ft, tag);
		}

		public static void ShowOverridingPreviusInstances (this DialogFragment dialog, FragmentManager fragmentManager, string tag)
		{
			var ft = fragmentManager.BeginTransaction ();
			var prev = fragmentManager.FindFragmentByTag (tag);
			if (prev != null)
				ft.Remove (prev);

			ft.AddToBackStack (null);
			dialog.Show (ft, tag);
		}

		public static bool TryDismiss (this DialogFragment dialog)
		{
			try {
				dialog.Dismiss ();
				return true;
			}
			catch(Exception) {
				return false;
			}
		}
	}
}

