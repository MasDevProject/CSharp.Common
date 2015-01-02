using System;
using Android.Locations;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.OS;

namespace MasDev.Droid.Utils
{
	public static class SensorsUtils
	{
		public static bool LocalizationEnabled (Context ctx, LocationManager manager = null)
		{
			bool gpsEnabled = false;
			bool networkEnabled = false;

			manager = manager ?? (LocationManager)ctx.GetSystemService (Context.LocationService);
			try {
				gpsEnabled = manager.IsProviderEnabled (LocationManager.GpsProvider);
			} catch {
			}
			try {
				networkEnabled = manager.IsProviderEnabled (LocationManager.NetworkProvider);
			} catch {
			}

			return gpsEnabled || networkEnabled;
		}

		public static bool IsGpsSensorEnabled (Context ctx)
		{
			var lm = (LocationManager)ctx.GetSystemService (Context.LocationService);
			return lm.IsProviderEnabled (LocationManager.GpsProvider);
		}

		public static bool IsNetworkLocalizationEnabled (Context ctx)
		{
			var lm = (LocationManager)ctx.GetSystemService (Context.LocationService);
			return lm.IsProviderEnabled (LocationManager.NetworkProvider);
		}

		public static void GetCurrentPositionUsingSingleRequest (Context ctx, Action<LatLng> onPositionRetrived, Action onTimeOut, Action onLocalizationDisabled, Accuracy accuracy = Accuracy.Medium, int timeOutInMillis = 10000, LocationManager manager = null)
		{
			if (manager == null)
				manager = (LocationManager)ctx.GetSystemService (Context.LocationService);
			if (!LocalizationEnabled (ctx, manager)) {
				onLocalizationDisabled.Invoke ();
				return;
			}

			var crit = new Criteria ();
			crit.Accuracy = accuracy;
			manager.RequestSingleUpdate (crit, new PositionUpdateListsner (onPositionRetrived, onTimeOut, timeOutInMillis), Looper.MyLooper ());	
		}
	}

	class PositionUpdateListsner : Java.Lang.Object, ILocationListener
	{
		Action _onTimeout;
		Action<LatLng> _onLatLangRetrived;
		bool _ignoreCallback;
		bool _positionRetrived;
		readonly int _timeOutMillis;

		public PositionUpdateListsner (Action<LatLng> onLatLangRetrived, Action onTimeOut, int timeOutInMillis)
		{
			_onLatLangRetrived = onLatLangRetrived;
			_timeOutMillis = timeOutInMillis;
			_onTimeout = onTimeOut;

			new Handler ().PostDelayed (OnTimeOut, _timeOutMillis);
		}

		void OnTimeOut ()
		{
			if (_positionRetrived)
				return;

			_ignoreCallback = true;
			_onTimeout.Invoke ();
		}

		public void OnLocationChanged (Location location)
		{
			if (_ignoreCallback)
				return;

			_positionRetrived = true;
			_onLatLangRetrived.Invoke (new LatLng (location.Latitude, location.Longitude));
		}

		public void OnProviderDisabled (string provider)
		{
		}

		public void OnProviderEnabled (string provider)
		{
		}

		public void OnStatusChanged (string provider, Availability status, Bundle extras)
		{
		}
	}
}

