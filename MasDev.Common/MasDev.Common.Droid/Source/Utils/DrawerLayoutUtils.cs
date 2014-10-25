using Android.Support.V4.Widget;
using Android.Support.V4.App;

namespace MasDev.Common.Droid.Utils
{
	public static class DrawerLayoutUtils
	{
		public static void CloseIfOpen (DrawerLayout drawerLayout, int gravity)
		{
			if (drawerLayout.IsDrawerOpen (gravity))
				drawerLayout.CloseDrawer (gravity);
		}

		public static bool IsOpen (DrawerLayout drawerLayout, int gravity)
		{
			return drawerLayout.IsDrawerOpen (gravity);
		}

		public static void Close (DrawerLayout drawerLayout, int gravity)
		{
			drawerLayout.CloseDrawer (gravity);
		}

		public static void Toggle (DrawerLayout drawerLayout, int gravity)
		{
			if (!drawerLayout.IsDrawerOpen (gravity))
				drawerLayout.OpenDrawer (gravity);
			else
				drawerLayout.CloseDrawer (gravity);
		}

		public static void LockSideMenuAndHideIndicator (DrawerLayout drawerLayout, ActionBarDrawerToggle drawerToggle)
		{
			drawerToggle.DrawerIndicatorEnabled = false;
			drawerLayout.SetDrawerLockMode (DrawerLayout.LockModeLockedClosed);
		}

		public static void UnLockSideMenuAndShowIndicator (DrawerLayout drawerLayout, ActionBarDrawerToggle drawerToggle)
		{
			drawerToggle.DrawerIndicatorEnabled = true;
			drawerLayout.SetDrawerLockMode (DrawerLayout.LockModeUnlocked);
		}

		public static void Lock (DrawerLayout drawerLayout)
		{
			drawerLayout.SetDrawerLockMode (DrawerLayout.LockModeLockedClosed);
		}

		public static void UnLock (DrawerLayout drawerLayout)
		{
			drawerLayout.SetDrawerLockMode (DrawerLayout.LockModeUnlocked);
		}
	}
}

