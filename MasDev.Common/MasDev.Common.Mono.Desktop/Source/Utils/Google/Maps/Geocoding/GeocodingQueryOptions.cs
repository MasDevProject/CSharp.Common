

namespace MasDev.Common.Utils.GoogleServices.Maps
{
	public class GeocodingQueryOptions
	{
		public bool Sensor { get; set; }



		public static GeocodingQueryOptions Default
		{ 
			get {
				return new GeocodingQueryOptions {
					Sensor = false,
				};
			}
		}
	}
}

