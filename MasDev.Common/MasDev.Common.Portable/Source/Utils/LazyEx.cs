using System;


namespace MasDev.Common.Utils
{
	public static class LazyEx
	{
		public static Lazy<T> Create<T> (Func<T> func)
		{
			return new Lazy<T> (func);
		}



		public static Lazy<T> Create<T> (Func<T> func, bool isThreadSafe)
		{
			return new Lazy<T> (func, isThreadSafe);
		}
	}
}

