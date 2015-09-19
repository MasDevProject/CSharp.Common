using System;
using MapKit;
using CoreLocation;
using CoreGraphics;

namespace MasDev.iOS.Utils
{
	public static class MKMapViewUtils
	{
		static double MERCATOR_OFFSET = 268435456;
		static double MERCATOR_RADIUS = 85445659.44705395;

		/// <summary>Converts miles to latitude degrees</summary>
		public static double MilesToLatitudeDegrees(double miles)
		{
			double earthRadius = 3960.0; // in miles
			double radiansToDegrees = 180.0/Math.PI;
			return (miles/earthRadius) * radiansToDegrees;
		}

		/// <summary>Converts miles to longitudinal degrees at a specified latitude</summary>
		public static double MilesToLongitudeDegrees(double miles, double atLatitude)
		{
			double earthRadius = 3960.0; // in miles
			double degreesToRadians = Math.PI/180.0;
			double radiansToDegrees = 180.0/Math.PI;
			// derive the earth's radius at that point in latitude
			double radiusAtLatitude = earthRadius * Math.Cos(atLatitude * degreesToRadians);
			return (miles / radiusAtLatitude) * radiansToDegrees;
		}

		/// <summary>Converts kilometres to latitude degrees</summary>
		public static double KilometresToLatitudeDegrees(double kms)
		{
			double earthRadius = 6371.0; // in kms
			double radiansToDegrees = 180.0/Math.PI;
			return (kms/earthRadius) * radiansToDegrees;
		}

		/// <summary>Converts kilometres to longitudinal degrees at a specified latitude</summary>
		public static double KilometresToLongitudeDegrees(double kms, double atLatitude)
		{
			double earthRadius = 6371.0; // in kms
			double degreesToRadians = Math.PI/180.0;
			double radiansToDegrees = 180.0/Math.PI;
			// derive the earth's radius at that point in latitude
			double radiusAtLatitude = earthRadius * Math.Cos(atLatitude * degreesToRadians);
			return (kms / radiusAtLatitude) * radiansToDegrees;
		}

		public static void CenterCoordinate (MKMapView MapToCenter, CLLocationCoordinate2D centerCoordinate, int zoomLevel, bool animated)
		{
			// clamp large numbers to 28
			zoomLevel = Math.Min (zoomLevel, 28);

			// use the zoom level to compute the region
			MKCoordinateSpan span = CoordinateSpanWithMapView (MapToCenter, centerCoordinate, zoomLevel);
			MKCoordinateRegion region = new MKCoordinateRegion (centerCoordinate, span);

			// set the region like normal
			MapToCenter.SetRegion (region, animated);
		}

		public static double LongitudeToPixelSpaceX (double longitude)
		{
			return Math.Round (MERCATOR_OFFSET + MERCATOR_RADIUS * longitude * Math.PI / 180.0);
		}

		public static double LatitudeToPixelSpaceY (double latitude)
		{
			return Math.Round (MERCATOR_OFFSET - MERCATOR_RADIUS * Math.Log ((1 + Math.Sin (latitude * Math.PI / 180.0)) / (1 - Math.Sin (latitude * Math.PI / 180.0))) / 2.0);
		}

		public static double PixelSpaceXToLongitude (double pixelX)
		{
			return ((Math.Round (pixelX) - MERCATOR_OFFSET) / MERCATOR_RADIUS) * 180.0 / Math.PI;
		}

		public static double PixelSpaceYToLatitude (double pixelY)
		{
			return (Math.PI / 2.0 - 2.0 * Math.Tan (Math.Exp ((Math.Round (pixelY) - MERCATOR_OFFSET) / MERCATOR_RADIUS))) * 180.0 / Math.PI;
		}

		public static MKCoordinateSpan CoordinateSpanWithMapView (MKMapView mapView, CLLocationCoordinate2D centerCoordinate, int zoomLevel)
		{
			// convert center coordiate to pixel space
			double centerPixelX = LongitudeToPixelSpaceX (centerCoordinate.Longitude);
			double centerPixelY = LatitudeToPixelSpaceY (centerCoordinate.Latitude);

			// determine the scale value from the zoom level
			int zoomExponent = 20 - zoomLevel;
			double zoomScale = Math.Pow (2, zoomExponent);

			// scale the map’s size in pixel space
			CGSize mapSizeInPixels = mapView.Bounds.Size;
			double scaledMapWidth = mapSizeInPixels.Width * zoomScale;
			double scaledMapHeight = mapSizeInPixels.Height;

			// figure out the position of the top-left pixel
			double topLeftPixelX = centerPixelX - (scaledMapWidth / 2);
			double topLeftPixelY = centerPixelY - (scaledMapHeight / 2);

			// find delta between left and right longitudes
			double minLng = PixelSpaceXToLongitude (topLeftPixelX);
			double maxLng = PixelSpaceXToLongitude (topLeftPixelX + scaledMapWidth);
			double longitudeDelta = maxLng - minLng;

			// find delta between top and bottom latitudes
			double minLat = PixelSpaceYToLatitude (topLeftPixelY);
			double maxLat = PixelSpaceYToLatitude (topLeftPixelY + scaledMapHeight);
			double latitudeDelta = -1 * (maxLat - minLat);

			// create and return the lat/lng span
			MKCoordinateSpan span = new MKCoordinateSpan (latitudeDelta, longitudeDelta);

			return span;
		}

		public static void ShowMapLocation(MKMapView mapView, CLLocationCoordinate2D coordinates)
		{
			ShowMapLocation (mapView, coordinates.Latitude, coordinates.Longitude);
		}

		public static void ShowMapLocation(MKMapView mapView, double latitude, double longitude)
		{
			CLLocationCoordinate2D coords = new CLLocationCoordinate2D (latitude, longitude);
			MKCoordinateSpan span = new MKCoordinateSpan(
				MKMapViewUtils.KilometresToLatitudeDegrees(3),
				MKMapViewUtils.KilometresToLongitudeDegrees(3, coords.Latitude));

			mapView.SetRegion (new MKCoordinateRegion (coords, span), true);
		}
	}
}