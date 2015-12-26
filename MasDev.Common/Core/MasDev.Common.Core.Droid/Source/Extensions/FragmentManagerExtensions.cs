using Android.Support.V4.App;

namespace MasDev.Droid.ExtensionMethods
{
	public static class FragmentManagerExtensions
	{
		public static T GetFragmentFromPagerAdapter<T> (this FragmentManager manager, int viewPagerId, int position) where T : class
		{
			return manager.FindFragmentByTag("android:switcher:" + viewPagerId + ":" + position) as T;
		}
	}
}

