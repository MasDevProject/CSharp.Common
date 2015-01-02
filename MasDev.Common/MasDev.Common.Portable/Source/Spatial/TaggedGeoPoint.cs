using MasDev.Spatial;


namespace MasDev.Spatial
{
	public class TaggedGeoPoint<T> : GeoPoint
	{
		public T Tag { get; set; }



		public TaggedGeoPoint (T tag, double latitude, double longitude) : base (latitude, longitude)
		{
			Tag = tag;
		}
	}
}

