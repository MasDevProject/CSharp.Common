using Android.Gms.Common;
using Android.App;
using MasDev.Patterns.Injection;
using MasDev.Utils;

namespace MasDev.Common
{
	public static class PlayServicesUtils
	{
		public enum CheckPlayServicesResponse
		{
			Ok,
			DeviceNotSupported,
			NotAvailable_DialogShown
		}

		public static int PLAY_SERVICES_RESOLUTION_REQUEST = 221;

		public static CheckPlayServicesResponse CheckPlayServices (Activity activity) 
		{
			var googleAvailability = GoogleApiAvailability.Instance;
			int resultCode = googleAvailability.IsGooglePlayServicesAvailable (activity);
			if (resultCode != ConnectionResult.Success) 
			{
				if (googleAvailability.IsUserResolvableError (resultCode)) {
					googleAvailability.GetErrorDialog (activity, resultCode, PLAY_SERVICES_RESOLUTION_REQUEST).Show ();
					return CheckPlayServicesResponse.NotAvailable_DialogShown;
				} 
				else {
					var logger = Injector.Resolve<ILogger> ();
					if (logger != null)
						logger.Log ("Device not supported");
					return CheckPlayServicesResponse.DeviceNotSupported;
				}
			}
			return CheckPlayServicesResponse.Ok;
		}
	}
}

