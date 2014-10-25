using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace MasDev.Common.Utils
{
	public static class Assert
	{
		public static T NotNull<T> (T arg) where T : class
		{
			if (arg == null)
				throw new ArgumentNullException ();
			return arg;
		}



		public static TCollection NotNullOrEmpty<TCollection> (TCollection collection) where TCollection : class, ICollection
		{
			if (NotNull (collection).Count == 0)
				throw new ArgumentException ("Argument is empty");
			return collection;
		}



		public static void Eq (object obj1, object obj2)
		{
			bool eq;

			if (obj1 == null && obj2 == null)
				eq = true;
			else if (obj1 == null && obj2 != null)
				eq = false;
			else if (obj1 != null && obj2 == null)
				eq = false;
			else
				eq = obj1.Equals (obj2);

			if (!eq)
				throw new AssertionViolatedException ("Objects are not equal");
		}



		public static T As<T> (object obj) where T : class
		{
			var t = obj as T;
			if (t == null)
				throw new AssertionViolatedException ("Pameter must be of type " + typeof(T));

			return t;
		}



		public static void True (bool condition)
		{
			if (!condition)
				throw new AssertionViolatedException ("Condition violated");
		}



		public static void HasFlag (int sourceFlag, int destFlag)
		{
			if (!FlagUtils.Has (sourceFlag, destFlag))
				throw new AssertionViolatedException ("Source flag does not contain destination flag");
		}
	}





	public class AssertionViolatedException : Exception
	{
		public AssertionViolatedException (string message) : base (message)
		{

		}
	}





	public static class Check
	{
		public static bool IsNull<T> (T what) where T : class
		{
			return what == null;
		}



		public static bool IsNotNull<T> (T what) where T : class
		{
			return what != null;
		}



		public static bool IsNullOrEmpty<T> (ICollection<T> collection)
		{
			return (collection == null || collection.Count == 0);
		}



		public static bool BothNotNull (object o1, object o2)
		{
			if (o1 == null)
				return false;
			return o2 != null;
		}



		public static bool BothNull (object o1, object o2)
		{
			if (o1 != null)
				return false;
			return o2 == null;
		}



		public static bool BoxedEquls (params object[] items)
		{
			var bools = Assert.NotNullOrEmpty (items).Select (i => i == null).ToList ();
			return bools.All (b => b) || bools.All (b => !b);
		}
	}
}

