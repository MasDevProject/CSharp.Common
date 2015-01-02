using System.Collections.Generic;
using System;
using System.Linq;
using MasDev.Spatial;


namespace MasDev.Spatial
{
	public class Clusterizer<T>
	{
		const double THRESHOLD = 0.0095d;


		readonly GeoArea _area;
		readonly ISet<Cluster<List<TaggedGeoPoint<T>>>> _clusters;
		readonly ICollection<TaggedClusteriableGeoPoint<T>> _points;
		readonly double _clusteringFactor;


		readonly Dictionary<TaggedClusteriableGeoPoint<T>, int> _idTable;
		readonly Dictionary<int, Cluster<List<TaggedGeoPoint<T>>>> _lookupTable;



		public Clusterizer (ICollection<TaggedGeoPoint<T>> points, GeoArea area, decimal clusteringFactor)
		{
			_area = area;
			_clusters = new HashSet<Cluster<List<TaggedGeoPoint<T>>>> ();
			_points = points.Select (p => new TaggedClusteriableGeoPoint<T> (p.Tag, p.Latitude, p.Longitude)).ToList ();
			_idTable = new Dictionary<TaggedClusteriableGeoPoint<T>, int> ();
			_lookupTable = new Dictionary<int, Cluster<List<TaggedGeoPoint<T>>>> ();
			_clusteringFactor = (7 / (double)clusteringFactor) * (7 / (double)clusteringFactor);

			var id = 0;
			foreach (var point in _points)
			{
				var cluster = new Cluster<List<TaggedGeoPoint<T>>> ();
				cluster.Centroid = new GeoPoint (point.Latitude, point.Longitude);
				cluster.Size = 0;
				cluster.Content = new List<TaggedGeoPoint<T>> { point };
				_clusters.Add (cluster);
				_idTable.Add (point, ++id);
				_lookupTable.Add (id, cluster);
			}
		}




		void Clusterize ()
		{
			var threshold = GeoUtils.DistanceInKm (_area.SouthWest, _area.NorthEast) * THRESHOLD * _clusteringFactor;

			foreach (var point in _points)
			{
				if (point.IsClusterized)
					continue;

				var mergiable = _clusters.FirstOrDefault (c => GeoUtils.DistanceInKm (point, c.Centroid) < threshold);
				if (mergiable == null || (mergiable.Centroid == point && mergiable.Content.Count == 1))
					continue;

				var pointId = _idTable [point];
				var singletonCluster = _lookupTable [pointId];
				_clusters.Remove (singletonCluster);

				if (mergiable.Content.Count == 1)
					_points.Single (p => p == point).IsClusterized = true;

				mergiable.Content.Add (point);
				point.IsClusterized = true;

				mergiable.Centroid = GeoUtils.GetCentroid (mergiable.Content);
			}
		}



		public List<Cluster<T>> GetClusters ()
		{
			Clusterize ();

			return _clusters.Select (c => new Cluster<T> {
				Centroid = c.Centroid,
				Content = c.Content.Count > 1 ? default(T) : c.Content.Single ().Tag,
				Size = c.Content.Count
			}).ToList ();
		}
	}





	class TaggedClusteriableGeoPoint<T> : TaggedGeoPoint<T>
	{
		public TaggedClusteriableGeoPoint (T tag, double latitude, double longitude) : base (tag, latitude, longitude)
		{
			IsClusterized = false;
		}



		public bool IsClusterized { get; set; }
	}
}

