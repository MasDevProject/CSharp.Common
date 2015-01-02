using System;
using System.Collections.Generic;
using MasDev.Spatial;
using System.Linq;


namespace MasDev.Spatial
{
	public class LayerCache<T>
	{
		readonly Dictionary<int, Layer<T>> _cache;
		readonly Queue<int> _layersQueue;
		readonly int _maxItemsCount;
		readonly int _maxLayersCount;
		int _itemsCount;



		public LayerCache (int maxItemsCount, int maxLayersCount = int.MaxValue)
		{
			_maxItemsCount = maxItemsCount;
			_maxLayersCount = maxLayersCount;
			_cache = new Dictionary<int, Layer<T>> ();
			_layersQueue = new Queue<int> ();
			_itemsCount = 0;
		}



		public bool IsCached (int layerId, GeoArea area)
		{
			var layer = FindLayer (layerId);
			return layer != null && layer.IsCached (area);
		}



		Layer<T> FindLayer (int layerId)
		{
			return !_cache.ContainsKey (layerId) ? null : _cache [layerId];
		}



		public ICollection<Cluster<T>> this [int layerId, GeoArea area]
		{
			get {
				var layer = FindLayer (layerId);
				if (layer == null || !layer.IsCached (area))
					return null;

				return layer [area].Where (c => area.Contains (c.Centroid)).ToList ();
			}


			set {
				if (IsCached (layerId, area))
					return;

				lock (_cache)
				{
					var layer = _cache.ContainsKey (layerId) ? _cache [layerId] : null;
					if (layer == null)
					{
						if (_layersQueue.Count == _maxLayersCount)
							throw new ArgumentException ("At most " + _maxItemsCount + " layers are allowed");

						layer = new Layer<T> ();
						_cache.Add (layerId, layer);
					}

					if (layer.Size == 0)
						_layersQueue.Enqueue (layerId);

					_itemsCount += layer.Add (area, value);
					if (_itemsCount > _maxItemsCount)
						Dequeue (_itemsCount - _maxItemsCount);
				}
			}
		}



		void Dequeue (int itemsToDequeueCount)
		{
			var itemsDequeued = 0;
			while (itemsDequeued < itemsToDequeueCount && _layersQueue.Any ())
			{
				var oldestLayer = _cache [_layersQueue.Peek ()];
				itemsDequeued += oldestLayer.Dequeue (itemsToDequeueCount);
				if (oldestLayer.Size == 0)
					_layersQueue.Dequeue ();
			}
		}
	}





	class Layer<T>
	{
		readonly Queue<ClusteredArea<T>> _cache;
		int _size;



		public int Size { get { return _size; } }



		public Layer ()
		{
			_size = 0;
			_cache = new Queue<ClusteredArea<T>> ();
		}



		public bool IsCached (GeoArea area)
		{
			return _cache.Any (ca => ca.Area.Contains (area));
		}



		public int Add (GeoArea area, ICollection<Cluster<T>> clusters)
		{
			_cache.Enqueue (new ClusteredArea<T> (area, clusters));
			_size += clusters.Count;
			return _size;
		}



		public ICollection<Cluster<T>> this [GeoArea area]
		{
			get {
				var clusteredArea = _cache.FirstOrDefault (ca => ca.Area.Contains (area));
				return clusteredArea == null ? null : clusteredArea.Clusters.ToList ();
			}

			set {
				if (IsCached (area))
					return;

				Enqueue (area, value);
			}
		}



		void Enqueue (GeoArea area, ICollection<Cluster<T>> clusters)
		{
			lock (_cache)
			{
				var clustersCount = clusters.Count;
				_size += clustersCount;
				_cache.Enqueue (new ClusteredArea<T> (area, clusters));
			}
		}



		public int Dequeue (int itemsToDequeueCount)
		{
			lock (_cache)
			{
				var dequeuedItemsCount = 0;
				while (dequeuedItemsCount < itemsToDequeueCount && _cache.Any ())
				{
					var oldestAddedClusteredArea = _cache.Dequeue ();
					dequeuedItemsCount += oldestAddedClusteredArea.Clusters.Count;
				}
				_size -= dequeuedItemsCount;
				return dequeuedItemsCount;
			}
		}
	}





	class ClusteredArea<T>
	{
		public GeoArea Area { get; private set; }



		public ICollection<Cluster<T>> Clusters { get; private set; }



		public ClusteredArea (GeoArea area)
		{
			Area = area;
			Clusters = new List<Cluster<T>> ();
		}



		public ClusteredArea (GeoArea area, ICollection<Cluster<T>> clusters) : this (area)
		{
			Clusters = clusters ?? new List<Cluster<T>> ();
		}
	}
}