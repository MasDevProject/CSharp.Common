

namespace MasDev.Spatial
{
	public class Cluster<T>
	{
		public GeoPoint Centroid { get; set; }



		public int Size { get; set; }



		public T Content { get; set; }



		public GeoArea Region { get; set; }
	}
}

