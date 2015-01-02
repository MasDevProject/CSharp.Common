using Android.Locations;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content;
using System.Linq;
using Android.Gms.Maps.Model;

namespace MasDev.Droid.Utils
{
	public static class GeoUtils
	{
		/// <summary>
		/// Gets the geo info from an string query. The following permissions are mandatory using this method:
		/// <uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" /> 
		/// <uses-permission android:name="android.permission.INTERNET" />
		/// 
		/// If the internet connection is unavailable, an exception is throwed.
		/// If the query does not have a result, a void list (not null) is returned.
		/// 
		/// </summary>
		/// <returns>A list of Address</returns>
		/// <param name="input">Input.</param>
		public static async Task<IList<Address>> GetGeoInfoFromAddress(Context ctx, string input, int maxResult) 
		{ 
			return await new Geocoder (ctx).GetFromLocationNameAsync (input, maxResult) ?? new List<Address> ();
		}

		public static async Task<IList<Address>> GetGeoInfoFromAddress(Context ctx, IEnumerable<char> input, int maxResult = 8) 
		{ 
			return await GetGeoInfoFromAddress (ctx, new string(input.ToArray()), maxResult);
		}

		public static async Task<IList<Address>> GetAddressByLatLang (Context ctx, LatLng latLng, int maxResult = 8)
		{
			return await new Geocoder (ctx).GetFromLocationAsync (latLng.Latitude, latLng.Longitude, maxResult);
		}

		public static float CalculareDistanceInMeters (LatLng startingPoint, LatLng endPoint)
		{
			var results = new float[1];
			Location.DistanceBetween (startingPoint.Latitude, startingPoint.Longitude, endPoint.Latitude, endPoint.Longitude, results);
			return results [0];
		}

		public static float CalculareDistanceInMeters (double startingPointLatitude, double startingPointLongitude, double endPointLatitude, double endPointLongitude)
		{
			var results = new float[1];
			Location.DistanceBetween (startingPointLatitude, startingPointLongitude, endPointLatitude, endPointLongitude, results);
			return results [0];
		}
	}
}

