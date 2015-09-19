using System;
using CoreLocation;
using MapKit;
using CoreGraphics;
using MasDev.iOS.Utils;

namespace MasDev.iOS.Extensions
{
	public static class MKMapViewExtensions
	{
		public static CLLocationCoordinate2D NECoordinates(this MKMapView mapView)
		{
			return mapView.ConvertPoint(
				new CGPoint(
					mapView.Bounds.X + mapView.Bounds.Width,
					mapView.Bounds.Y),
				mapView);
		}

		public static CLLocationCoordinate2D SWCoordinates(this MKMapView mapView)
		{
			return mapView.ConvertPoint(
				new CGPoint(
					mapView.Bounds.X,
					mapView.Bounds.Y + mapView.Bounds.Height),
				mapView);
		}

		public static int ZoomLevel(this MKMapView mapView)
		{
			var region = mapView.Region;

			var centerPixelX = MKMapViewUtils.LongitudeToPixelSpaceX(region.Center.Longitude);
			var topLeftPixelX = MKMapViewUtils.LongitudeToPixelSpaceX(region.Center.Longitude - region.Span.LongitudeDelta / 2);

			var scaledMapWidth = (centerPixelX - topLeftPixelX) * 2;
			var mapSizeInPixels = mapView.Bounds.Size;
			var zoomScale = scaledMapWidth / mapSizeInPixels.Width;
			var zoomExponent = Math.Log(zoomScale) / Math.Log(2);
			var zoomLevel = 20 - zoomExponent;

			return (int) zoomLevel;
		}
	}
}