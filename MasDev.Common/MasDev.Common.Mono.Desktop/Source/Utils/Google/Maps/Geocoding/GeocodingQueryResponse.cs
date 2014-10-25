using System.Collections.Generic;
using MasDev.Common.Spatial;


namespace MasDev.Common.Utils.GoogleServices.Maps
{
	public class GeocodingQueryResponse
	{
		public ServiceResponseStatus Status { get; set; }



		public ICollection<GeocodingQueryResult> Results { get; set; }
	}





	public class GeocodingQueryResult
	{
		public string FormattedAddress { get; set; }



		public GeoPoint Coordinates { get; set; }
	}
}

