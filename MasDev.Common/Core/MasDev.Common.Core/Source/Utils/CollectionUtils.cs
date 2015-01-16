using System.Collections.Generic;
using System.Linq;
using System;


namespace MasDev.Utils
{
	public delegate bool Comparer<T> (T o1, T o2);
	public delegate T Cloner<T> (T t);

	public static class CollectionUtils
	{
		public static bool IsNullOrEmpty<T> (IEnumerable<T> coll)
		{
			return coll == null || !coll.Any ();
		}



		public static bool Eq<T> (IEnumerable<T> c1, IEnumerable<T> c2, Comparer<T> comparer)
		{
			if (c1 == null && c2 == null)
				return true;

			if (c1 != null && c2 == null)
				return false;

			if (c1 == null && c2 != null)
				return false;

			var a1 = c1.ToList ();
			var a2 = c2.ToList ();

			if (a1.Count != a2.Count)
				return false;

			var difference = a1.Minus (a2, comparer).ToList ();
			return !difference.Any ();
		}



		public static IEnumerable<T> Minus<T> (this IEnumerable<T> source, IEnumerable<T> what, Comparer<T> comparer)
		{
			var v = what.ToList ();
			foreach (var t in source)
				if (v.All (i => !comparer (t, i)))
					yield return t;
		}



		public static IEnumerable<T> Minus<T> (this IEnumerable<T> source, IEnumerable<T> what, Comparer<T> comparer, Cloner<T> cloner)
		{
			var v = what.ToList ();
			foreach (var t in source)
				if (v.All (i => !comparer (t, i)))
					yield return cloner (t);
		}



		public static bool AllNullOrEmpty<T> (params IEnumerable<T>[] ienumerables)
		{
			return Assert.NotNullOrEmpty<IEnumerable<T>[], IEnumerable<T>> (ienumerables).All (i => IsNullOrEmpty (i));
		}


		public static bool AllNullOrEmpty (params IEnumerable<object>[] ienumerables)
		{
			return Assert.NotNullOrEmpty<IEnumerable<object>[], IEnumerable<object>> (ienumerables).All (IsNullOrEmpty);
		}


		public static bool BoxedEquals<T> (params IEnumerable<T>[] ienumerables)
		{
			var lists = Assert.NotNull (ienumerables).Select (i => i == null ? null : i.ToList ()).ToList ();
			var bools = lists.Select (i => i == null).ToList ();
			if (!(bools.All (b => b) || bools.All (b => !b)))
				return false;

			var first = lists [0];
			if (first == null)
				return true;


			bools = lists.Select (i => i.Any ()).ToList ();
			if (!(bools.All (b => b) || bools.All (b => !b)))
				return false;

			if (!first.Any ())
				return true;

			var count = first.Count ();
			return lists.All (i => i.Count () == count);
		}



		public static void AddIfNotPresent<T> (ref ICollection<T> collection, T element, Func<T, T, bool> containsPreticate)
		{
			collection = collection ?? new List<T> ();
			if (collection.All (p => !containsPreticate (p, element)))
				collection.Add (element);
		}

		public static void AddIfNotPresent<T> (ref ICollection<T> collection, T element)
		{
			AddIfNotPresent (ref collection, element, (o1, o2) => !Check.NullSafeEquals (o1, o2));
		}
	}
}

