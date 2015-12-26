using Android.Support.V4.App;

namespace MasDev.Droid.ExtensionMethods
{
	public static class FragmentTransactionExtensions
	{
		public static FragmentTransaction ReplaceIfNotExists (this FragmentTransaction transaction, FragmentManager fragmentManager, int containerId, Fragment fragment, string tag)
		{
			if (fragmentManager.FindFragmentByTag (tag) == null)
				transaction.Replace (containerId, fragment, tag);

			return transaction;
		}

		public static FragmentTransaction AddIfNotExists (this FragmentTransaction transaction, FragmentManager fragmentManager, int containerId, Fragment fragment, string tag)
		{
			if (fragmentManager.FindFragmentByTag (tag) == null)
				transaction.Add (containerId, fragment, tag);

			return transaction;
		}
	}
}

