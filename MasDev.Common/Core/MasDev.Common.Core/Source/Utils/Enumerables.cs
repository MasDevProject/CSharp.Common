using System;
using System.Collections.Generic;

namespace MasDev.Common
{
	public static class Enumerables
	{
		public static IEnumerable<T> Single<T> (T item)
		{
			yield return item;
		}
	}
}

