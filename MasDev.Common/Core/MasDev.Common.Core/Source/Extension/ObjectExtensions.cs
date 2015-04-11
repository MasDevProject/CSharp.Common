using System;
using AutoMapper;


namespace MasDev.Extensions
{
	public static class ObjectExtensions
	{
		public static bool IsSameClass (this object obj, object obj2)
		{
			if (obj2 == null)
				return false;

			return obj.GetType () == obj2.GetType ();
		}



		public static TKey Safety<TKey, TSource> (this TSource obj, Func<TSource, TKey> property) where TSource : class
		{
			return obj == null ? default(TKey) : property (obj);
		}


		public static void Safety<TSource> (this TSource obj, Action<TSource> voidMethodCall) where TSource : class
		{
			if (obj == null)
				return;

			voidMethodCall (obj);
		}

		public static T CloneObject<T> (this T obj)
		{
			return Mapper.DynamicMap<T> (obj);
		}
	}
}

