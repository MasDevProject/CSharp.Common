using Android.Gms.Common;
using Android.App;
using MasDev.Patterns.Injection;
using MasDev.Utils;


namespace MasDev.Common
{
	public static class PlayServicesUtils
	{
		public static int PLAY_SERVICES_RESOLUTION_REQUEST = 1234567;

		public static bool CheckPlayServices (Activity activity) 
		{
			int resultCode = GooglePlayServicesUtil.IsGooglePlayServicesAvailable(activity);
			if (resultCode != ConnectionResult.Success) 
			{
				if (GooglePlayServicesUtil.IsUserRecoverableError (resultCode)) {
					GooglePlayServicesUtil.GetErrorDialog(resultCode, activity, PLAY_SERVICES_RESOLUTION_REQUEST).Show();
				} else {
					var logger = Injector.Resolve<ILogger> ();
					if (logger != null)
						logger.Log ("Device not supported");
					activity.Finish ();
				}
				return false;
			}
			return true;
		}
	}
}

