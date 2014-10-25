using MasDev.Common.Utils.GoogleServices.Maps;
using MasDev.Common.Utils.GoogleServices.CloudMessaging;


namespace MasDev.Common.Utils.GoogleServices
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

