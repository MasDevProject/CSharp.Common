using System;


namespace MasDev.Spatial
{
	public sealed class GeoArea
	{
		public GeoPoint SouthWest { get; set; }



		public GeoPoint NorthEast { get; set; }



		public GeoArea ()
		{
		}



		public GeoArea (GeoPoint southWest, GeoPoint northEast)
		{
			SouthWest = southWest;
			NorthEast = northEast;
		}



		public GeoArea (double southWestLatitude, double southWestLongitude, double nothEastLatitude, double nothEastLongitude)
		{
			SouthWest = new GeoPoint (southWestLatitude, southWestLongitude);
			NorthEast = new GeoPoint (nothEastLatitude, nothEastLongitude);
		}



		public bool Contains (GeoPoint point)
		{
			return Contains (point.Latitude, point.Longitude);
		}



		public bool Contains (double latitude, double longitude)
		{
			var inX = latitude >= SouthWest.Latitude && latitude <= NorthEast.Latitude;
			var inY = longitude >= SouthWest.Longitude && longitude <= NorthEast.Longitude;
			return inX && inY;
		}



		public bool Contains (GeoArea area)
		{
			return IsAboveSouthWestCorner (area.SouthWest) && IsBelowNorthEstCorner (area.NorthEast);
		}



		public bool IsAboveSouthWestCorner (GeoPoint point)
		{
			return point.Latitude >= SouthWest.Latitude && point.Longitude >= SouthWest.Longitude;
		}



		public GeoArea Grow (double p)
		{
			return new GeoArea (SouthWest.Latitude - p, SouthWest.Longitude - p, NorthEast.Latitude + p, NorthEast.Longitude + p);
		}



		public bool IsBelowNorthEstCorner (GeoPoint point)
		{
			return point.Latitude <= NorthEast.Latitude && point.Longitude <= NorthEast.Longitude;
		}



		public double Width
		{ get { return NorthEast.Latitude - SouthWest.Latitude; } }



		public double Height
		{ get { return NorthEast.Longitude - SouthWest.Longitude; } }



		public GeoArea Intersection (GeoArea area)
		{
			var a = SouthWest;
			var b = area.SouthWest;
			var x1 = Math.Max (a.Latitude, b.Latitude);
			var x2 = Math.Min (a.Latitude + Width, b.Latitude + area.Width);
			var y1 = Math.Max (a.Longitude, b.Longitude);
			var y2 = Math.Min (a.Longitude + Height, b.Longitude + area.Height);

			if (x2 >= x1 && y2 >= y1)
				return new GeoArea (x1, y1, x2, y2);

			return GeoArea.Empty;
		}



		public override string ToString ()
		{
			return string.Format ("SouthWestLat={0}, SouthWestLong={1}\t NorthEastLat={2}, NorthEastLong={3}", SouthWest.Latitude, SouthWest.Longitude, NorthEast.Latitude, NorthEast.Longitude);
		}



		public static readonly GeoArea Empty = new GeoArea (double.MinValue, double.MinValue, double.MinValue, double.MinValue);



		public override bool Equals (object obj)
		{
			try
			{
				var casted = (GeoArea)obj;
				return casted.SouthWest == SouthWest && casted.NorthEast == NorthEast;
			} catch (Exception)
			{
				return false;
			}
		}



		public static bool operator == (GeoArea a, GeoArea b)
		{
			return a.Equals (b);
		}



		public static bool operator != (GeoArea a, GeoArea b)
		{
			return !a.Equals (b);
		}



		public static GeoArea operator - (GeoArea a, GeoArea b)
		{
			return a.Intersection (b);
		}



		public Tuple<GeoArea, GeoArea> GetProtrudingRectangles (GeoArea tocDeMerda)
		{
			var ultreMegaIperPowaRectangle = GetUltraMegaIperPowaRectangle (tocDeMerda);
			var a = ultreMegaIperPowaRectangle.SouthWest;
			var k = SouthWest;

			var r1Width = ultreMegaIperPowaRectangle.Width - Width;
			var r1Height = ultreMegaIperPowaRectangle.Height;

			var r1Latitude = a.Latitude < k.Latitude ? a.Latitude : k.Latitude + Width; 
			var r1Longitude = a.Longitude < k.Longitude ? a.Longitude : k.Longitude;

			var r1 = new GeoArea (r1Latitude, r1Longitude, r1Width + r1Latitude, r1Height + r1Longitude);


			var r2Width = Width; 
			var r2Height = ultreMegaIperPowaRectangle.Height - Height; 

			var r2Latitude = a.Latitude < k.Latitude ? k.Latitude : a.Latitude;
			var r2Longitude = a.Longitude < k.Longitude ? a.Longitude : k.Longitude + Height;

			var r2 = new GeoArea (r2Latitude, r2Longitude, r2Width + r2Latitude, r2Height + r2Longitude);

			return Tuple.Create (r1, r2);
		}



		public  Tuple<GeoArea, GeoArea> SplitHalf ()
		{
			var half = SouthWest.Latitude + (Width / 2) + 0.00000001d;
			var r1 = new GeoArea (SouthWest, new GeoPoint (half, NorthEast.Longitude));

			var r2 = new GeoArea (new GeoPoint (half, SouthWest.Longitude), NorthEast);

			return Tuple.Create (r1, r2);
		}



		public GeoArea GetUltraMegaIperPowaRectangle (GeoArea tocDeMerda)
		{
			var swLatitude = Math.Min (SouthWest.Latitude, tocDeMerda.SouthWest.Latitude);
			var swLongitute = Math.Min (SouthWest.Longitude, tocDeMerda.SouthWest.Longitude);

			var neLatitude = Math.Max (NorthEast.Latitude, tocDeMerda.NorthEast.Latitude);
			var neLongitude = Math.Max (NorthEast.Longitude, tocDeMerda.NorthEast.Longitude);

			return new GeoArea (swLatitude, swLongitute, neLatitude, neLongitude);
		}



		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
	}





	public class GeoPoint
	{
		public double Latitude { get; set; }



		public double Longitude { get; set; }



		const double EPSILON = 0.0000005d;



		public GeoPoint ()
		{

		}



		public GeoPoint (double latitude, double longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}



		public override bool Equals (object obj)
		{
			try
			{
				var casted = (GeoPoint)obj;
				return Math.Abs (casted.Latitude - casted.Latitude) < EPSILON && Math.Abs (casted.Longitude - Longitude) < EPSILON;
			} catch (Exception)
			{
				return false;
			}
		}



		public static bool operator == (GeoPoint a, GeoPoint b)
		{
			if (((Object)a) == null)
				return ((Object)b) == null;

			return a.Equals (b);
		}



		public static bool operator != (GeoPoint a, GeoPoint b)
		{
			if (((Object)a) == null)
				return ((Object)a) != null;
			return !a.Equals (b);
		}



		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}



		public override string ToString ()
		{
			return string.Format ("Lat: {0}, Long:{1}", Latitude, Longitude);
		}
	}
}

