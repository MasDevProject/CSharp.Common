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
			var googleAvailability = GoogleApiAvailability.Instance;
			int resultCode = googleAvailability.IsGooglePlayServicesAvailable(activity);
			if (resultCode != ConnectionResult.Success) 
			{
				if (googleAvailability.IsUserResolvableError (resultCode)) {
					googleAvailability.GetErrorDialog(activity, resultCode, PLAY_SERVICES_RESOLUTION_REQUEST).Show();
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

