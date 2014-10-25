using System;
using MasDev.Common.Spatial;
using System.Collections.Generic;
using System.Linq;


namespace MasDev.Common.Spatial
{
	public static class GeoUtils
	{
		const double EPSILON = 0.000000000000000001d;



		public static double GetMinDistanceInKm (GeoPoint point, ICollection<GeoPoint> points)
		{
			if (points == null)
				return 0;

			points = points.Where (p => p != null).ToList ();
			if (!points.Any ())
				return 0;
				
			var minDistance = double.MaxValue;
			foreach (var p in points)
			{
				if (Math.Abs (p.Latitude - point.Latitude) < EPSILON && Math.Abs (p.Longitude - p.Longitude) < EPSILON)
					continue;

				var distance = DistanceInKm (point, p);
				if (distance < minDistance)
					minDistance = distance;
			}

			return minDistance;
		}



		public static double DistanceInKm (double lat1, double long1, double lat2, double long2)
		{
			return GetDistance (lat1, long1, lat2, long2, 'K');
		}



		public static double DistanceInKm (GeoPoint p1, GeoPoint p2)
		{
			return GetDistance (p1.Latitude, p1.Longitude, p2.Latitude, p2.Longitude, 'K');
		}



	
		static double GetDistance (double lat1, double lon1, double lat2, double lon2, char unit)
		{
			double theta = lon1 - lon2;
			double dist = Math.Sin (DegToRadiants (lat1)) * Math.Sin (DegToRadiants (lat2)) + Math.Cos (DegToRadiants (lat1)) * Math.Cos (DegToRadiants (lat2)) * Math.Cos (DegToRadiants (theta));
			dist = Math.Acos (dist);
			dist = RadiantsToDeg (dist);
			dist = dist * 60 * 1.1515;
			if (unit == 'K')
			{
				dist = dist * 1.609344;
			} else if (unit == 'N')
			{
				dist = dist * 0.8684;
			}
			return dist;
		}



		public static double DegToRadiants (double deg)
		{
			return (deg * Math.PI / 180.0);
		}



		public static double RadiantsToDeg (double rad)
		{
			return (rad / Math.PI * 180.0);
		}



		public static GeoPoint GetCentroid (params GeoPoint[] points)
		{
			return GetCentroid (points.AsEnumerable ());
		}



		public static GeoPoint GetCentroid (IEnumerable<GeoPoint> points)
		{
			var count = 0d;
			var latitude = 0d;
			var longitude = 0d;

			foreach (var point in points)
			{
				count++;
				latitude += point.Latitude;
				longitude += point.Longitude;
			}

			return new GeoPoint (latitude / count, longitude / count);
		}

	}
}

