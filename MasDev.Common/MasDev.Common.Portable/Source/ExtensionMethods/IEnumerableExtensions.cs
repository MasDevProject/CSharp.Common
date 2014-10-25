using System;
using System.Collections.Generic;
using System.Linq;


namespace MasDev.Common.Extensions
{
	public static class IEnumerableExtensions
	{
		static readonly Random _random = new Random ();



		public static void ForEach<T> (this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (var t in enumerable)
				action (t);
		}



		public static T RandomElement<T> (this T[] elements)
		{
			return elements [_random.Next (elements.Length)];
		}



		public static List<T> AsList<T> (this IEnumerable<T> enumerable)
		{
			return enumerable == null ? null : enumerable.ToList ();
		}



		public static IEnumerable<T> Concat<T> (this IEnumerable<T> ienum, T element)
		{
			foreach (var t in ienum)
				yield return t;
			yield return element;
		}



		public static IEnumerable<TResult> Select<TSource, TFunc, TResult> (this IEnumerable<TSource> ienum, Func<TSource, TFunc> func) where TFunc : TResult
		{
			return ienum.Select (func).OfType<TResult> ();
		}
	}
}

