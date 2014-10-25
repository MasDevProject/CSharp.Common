using System;
using Android.Content;

namespace MasDev.Common.Droid.App
{
	public class DrawerLayoutActivityConfiguration
	{
		/// <summary>
		/// Gets the fragment frame identifier.
		/// </summary>
		/// <value>The fragment frame identifier.</value>
		public int FragmentFrameId { get; protected set; }

		/// <summary>
		/// Gets the drawer layout identifier.
		/// </summary>
		/// <value>The drawer layout identifier.</value>
		public int DrawerLayoutId { get; protected set; }

		/// <summary>
		/// Gets the side menu left identifier.
		/// </summary>
		/// <value>The side menu left identifier.</value>
		public int SideMenuLeftId { get; protected set; }

		/// <summary>
		/// Gets the activity layout identifier.
		/// </summary>
		/// <value>The activity layout identifier.</value>
		public int ActivityLayoutId { get; protected set; }

		/// <summary>
		/// Gets the navigation drawer icon identifier.
		/// </summary>
		/// <value>The navigation drawer icon identifier.</value>
		public int NavigationDrawerIconId { get; protected set; }

		/// <summary>
		/// Gets the application name string identifier.
		/// </summary>
		/// <value>The application name string identifier.</value>
		public int ApplicationNameStringId { get; protected set; }

		public Action<Context> BackButtonPressOnRootFragmentAction { get; protected set; }

		public DrawerLayoutActivityConfiguration (int fragmentFrameId, int drawerLayoutId, int sideMenuLeftId, int activityLayoutId, int navigationDrawerIconId, int applicationNameStringId, Action<Context> actionOnBackPress)
		{
			FragmentFrameId = fragmentFrameId;
			DrawerLayoutId = drawerLayoutId;
			SideMenuLeftId = sideMenuLeftId;
			ActivityLayoutId = activityLayoutId;
			NavigationDrawerIconId = navigationDrawerIconId;
			ApplicationNameStringId = applicationNameStringId;
			BackButtonPressOnRootFragmentAction = actionOnBackPress;
		}
	}
}

