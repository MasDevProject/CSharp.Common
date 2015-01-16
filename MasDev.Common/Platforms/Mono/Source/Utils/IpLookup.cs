using System.Threading.Tasks;
using System.Net.Http;
using MasDev.Exceptions;
using System.Net;
using Newtonsoft.Json.Linq;
using MasDev.Spatial;


namespace MasDev.Utils
{
	public static class IpLookup
	{
		const string BASE_URL = "http://api.hostip.info/get_json.php?ip={0}&position=true";



		public static async Task<IpLookupResult> QueryAsync (string ip)
		{
			var url = string.Format (BASE_URL, ip);
			using (var client = new HttpClient ())
			{
				var response = await client.GetAsync (url);
				if (response.StatusCode != HttpStatusCode.OK)
					throw new HttpException (response.StatusCode);

				var rawJson = await response.Content.ReadAsStringAsync ();
				var json = JObject.Parse (rawJson);

			    double latitude = 0;
			    double longitude = 0;

			    double.TryParse(json["lat"].Value<string>(), out latitude);
			    double.TryParse(json["lng"].Value<string>(), out longitude);

				return new IpLookupResult {
					City = json ["city"].Value<string> (),
					CountryCode = json ["country_code"].Value<string> (),
					CountryName = json ["country_name"].Value<string> (),
					Ip = json ["ip"].Value<string> (),
					Position = new GeoPoint (
                        latitude,
                        longitude
					) 
				};
			}
		}
	}





	public class IpLookupResult
	{
		public string City { get; set; }



		public string CountryCode { get; set; }



		public string CountryName { get; set; }



		public string Ip { get; set; }



		public GeoPoint Position { get; set; }
	}
}

