using CoreLocation;
using System.Linq;
using System.Threading.Tasks;

namespace MasDev.iOS.Utils
{
	public static class CLGeocoderUtils
	{
		public static async Task<CLLocationCoordinate2D> GeocodeAddressAsync(string address)
		{
			var region = default(CLLocationCoordinate2D);

			if (string.IsNullOrWhiteSpace (address))
				return region;

			try
			{
				var geocoder = new CLGeocoder ();
				var res = await geocoder.GeocodeAddressAsync (address);

				if (res != null && res.Any ())
					region = res [0].Location.Coordinate;
			}
			catch
			{
				region = default(CLLocationCoordinate2D);
			}

			return region;
		}
	}
}