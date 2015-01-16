using MasDev.GoogleServices.Maps;
using MasDev.GoogleServices.CloudMessaging;


namespace MasDev.GoogleServices
{
	public static class Google
	{
		public static readonly MapsServices Maps;
		public static readonly CloudMessagingService Messaging;



		static Google ()
		{
			Maps = new MapsServices ();
			Messaging = new CloudMessagingService ();
		}
	}





	public class MapsServices
	{
		public readonly GeocoderService Geocoder;



		public MapsServices ()
		{
			Geocoder = new GeocoderService ();
		}
	}
}

