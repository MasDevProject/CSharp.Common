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
			if (!BitwiseUtils.Has (sourceFlag, destFlag))
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
		public static bool NullityEquals<T> (T o1, T o2)
		{
			EnsureIsNullable<T> ();
			if (BothNull (o1, o2))
				return false;

			return BothNotNull (o1, o2);
		}

		public static bool LeftNullityEquals<T> (T o1, T o2)
		{
			EnsureIsNullable<T> ();
			if (o1 != null)
				return true;

			return o2 == null;
		}


		public static bool IsPositiveOrNull<T> (T nullableNumber)
		{
			EnsureIsNullable<T> ();
			if (nullableNumber == null)
				return true;

			try {
				var stringlyNumber = nullableNumber.ToString ();
				var number = double.Parse (stringlyNumber);
				return number >= 0;
			} catch (Exception) {
				throw new ArgumentException ("Argument must be a number");
			}
		}


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


		public static bool NullSafeEquals<T> (T o1, T o2)
		{
			if (BothNotNull (o1, o2))
				return o1.Equals (o2);

			if (BothNull (o1, o2))
				return true;

			return false;
		}

		public static bool NullSafeEquals<T> (T o1, T o2, Func<T, T, bool> comparer)
		{
			if (BothNotNull (o1, o2))
				return comparer (o1, o2);

			if (BothNull (o1, o2))
				return true;

			return false;
		}


		public static bool BoxedEquls (params object[] items)
		{
			var bools = Assert.NotNullOrEmpty (items).Select (i => i == null).ToList ();
			return bools.All (b => b) || bools.All (b => !b);
		}


		public static bool NullSafeAllCollectionsEquals<T> (ICollection<T>[] collections)
		{
			return NullSafeAllCollectionsEquals ((a, b) => a.Equals (b), collections);
		}



		public static bool NullSafeAllCollectionsEquals<T> (Comparer<T> comparer, ICollection<T>[] collections)
		{
			var areCollectionsNull = Assert.NotNullOrEmpty (collections).Select (i => i == null).ToList ();

			// All null
			if (areCollectionsNull.All (isNull => isNull))
				return true;

			// Some null, some others not null
			if (areCollectionsNull.Any (isNull => isNull))
				return false;

			var collectionSize = collections [0].Count;
			if (!collections.All (item => item.Count == collectionSize))
				return false;

			var testCollection = collections [0];

			for (var i = 1; i < collections.Length; i++) {
				var collection = collections [i];
				var inTestButNotInCurrent = testCollection.Minus (collection, comparer).Count ();
				if (inTestButNotInCurrent > 0)
					return false;

				var inCollectionButNotInTest = collection.Minus (testCollection, comparer).Count ();
				if (inCollectionButNotInTest > 0)
					return false;
			}

			return true;
		}

		private static void EnsureIsNullable<T> ()
		{
			var type = typeof(T);

			if (Nullable.GetUnderlyingType (type) != null)
				return;

			if (type.IsByRef)
				return;

			throw new InvalidOperationException ("Type is not nullable or reference type.");
		}
	}
}

