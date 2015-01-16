using System;
using System.Collections.Generic;


namespace MasDev.Utils
{
	class EqualityComparer<T> : IEqualityComparer<T>
	{
		readonly Func<T, T, bool> _comparer;



		public EqualityComparer (Func<T, T, bool> comparer)
		{
			_comparer = comparer;
		}



		public bool Equals (T x, T y)
		{
			return _comparer (x, y);
		}



		public int GetHashCode (T obj)
		{
			return obj.GetHashCode ();
		}
	}





	public static class EqualityComparer
	{
		public static IEqualityComparer<T> Create<T> (Func<T, T, bool> comparer)
		{
			return new EqualityComparer<T> (comparer);
		}
	}





	class ComparerEx<T> : IComparer<T>
	{
		readonly Func<T, T, int> _comparer;



		public ComparerEx (Func<T, T, int> comparer)
		{
			_comparer = comparer;
		}



		public int Compare (T x, T y)
		{
			return _comparer (x, y);
		}
	}





	public static class ComparerEx
	{
		public static IComparer<T> Create<T> (Func<T, T, int> comparer)
		{
			return new ComparerEx<T> (comparer);
		}
	}
}

