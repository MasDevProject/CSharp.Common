using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using MasDev.Common.Spatial;


namespace MasDev.Common.Utils.GoogleServices.Maps
{
	public class GeocoderService
	{
		const string BASE_URL = "https://maps.googleapis.com/maps/api/geocode/json?address={0}&key={1}&sensor={2}";



		public async Task<GeocodingQueryResponse> QueryAsync (string address, string apiKey, GeocodingQueryOptions opts = null)
		{
			var options = opts ?? GeocodingQueryOptions.Default;
			var requestUrl = string.Format (BASE_URL,
				                 address,
				                 apiKey,
				                 options.Sensor
			                 );
		
			using (var client = new HttpClient ())
			{
				var response = await client.GetAsync (requestUrl);
				var json = await response.Content.ReadAsStringAsync ();
				var deserializedJson = JObject.Parse (json);
				var geoResponse = new GeocodingQueryResponse ();

				var status = deserializedJson ["status"].Value<string> ().ToLowerInvariant ();
				geoResponse.Status = ParseStatus (status
				);
				if (geoResponse.Status != ServiceResponseStatus.Ok)
					return geoResponse;

				var results = deserializedJson ["results"];
				geoResponse.Results = new List<GeocodingQueryResult> ();
				foreach (var result in results)
				{
					var formattedAddr = result ["formatted_address"].Value<string> ();
					var location = result ["geometry"] ["location"];
					var geoResult = new GeocodingQueryResult {
						Coordinates = new GeoPoint (
							double.Parse (location ["lat"].Value<string> ()),
							double.Parse (location ["lng"].Value<string> ())
						),
						FormattedAddress = formattedAddr
					};
					geoResponse.Results.Add (geoResult);
				}
				return geoResponse;
			}
		}



		static ServiceResponseStatus ParseStatus (string str)
		{
			switch (str)
			{
			case "ok":
				return ServiceResponseStatus.Ok;
			case "zero_results":
				return ServiceResponseStatus.ZeroResults;
			case "invalid_request":
				return ServiceResponseStatus.InvalidRequest;
			case "over_query_limit":
				return ServiceResponseStatus.OverQueryLimit;
			case "request_denied":
				return ServiceResponseStatus.RequestDenied;
			case "unknown":
				return ServiceResponseStatus.Unknown;

		
			default:
				throw new ArgumentException ("Unrecognized value" + str);
			}
		}
	}
}

