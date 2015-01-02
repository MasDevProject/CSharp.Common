using Android.Support.V4.Widget;

namespace MasDev.Droid.Utils
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

