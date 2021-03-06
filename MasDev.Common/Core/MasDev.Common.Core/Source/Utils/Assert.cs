﻿using System;
using System.Collections.Generic;
using System.Linq;


namespace MasDev.Utils
{
	public static class Assert
	{
		public static T NotNull<T> (T arg) where T : class
		{
			if (arg == null)
				throw new ArgumentNullException ();
			return arg;
		}



		public static TCollection NotNullOrEmpty<TCollection, T> (TCollection collection)where TCollection : class, ICollection<T>
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
			return !BothNull (o1, o2) && BothNotNull (o1, o2);

		}

		public static bool LeftNullityEquals<T> (T o1, T o2)
		{
			if (o1 != null)
				return true;
			return o2 == null;
		}


		#if !SALTARELLE
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
		#endif


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


		public static bool BlindEquals<T> (ICollection<T> c1, ICollection<T> c2)
		{
			if (IsNullOrEmpty (c1) && IsNullOrEmpty (c2))
				return true;

			if (IsNullOrEmpty (c1) && !IsNullOrEmpty (c2))
				return false;

			if (IsNullOrEmpty (c2) && !IsNullOrEmpty (c1))
				return false;

			return c1.Count == c2.Count;
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
		    if (BothNotNull(o1, o2))
		    {
		        var eq =o1.Equals(o2);
		        return eq;
		    }
		    var bothNull = BothNull (o1, o2);
		    return bothNull;
		}

		public static bool NullSafeEquals<T1, T2> (T1 o1, T1 o2, Func<T1, T2> property)
		{
			return BothNotNull (o1, o2) ? NullSafeEquals (property (o1), property (o2)) : BothNull (o1, o2);

		}

		public static bool NullSafeEquals<T> (T o1, T o2, Func<T, T, bool> comparer)
		{
			return BothNotNull (o1, o2) ? comparer (o1, o2) : BothNull (o1, o2);
		}


		public static bool BoxedEquls (params object[] items)
		{
			var bools = items.Select (i => i == null).ToList ();
			return bools.All (b => b) || bools.All (b => !b);
		}


		public static bool NullSafeAllCollectionsEquals<T> (ICollection<T>[] collections)
		{
			return NullSafeAllCollectionsEquals ((a, b) => a.Equals (b), collections);
		}



		public static bool NullSafeAllCollectionsEquals<T> (Comparer<T> comparer, ICollection<T>[] collections)
		{
			var areCollectionsNull = collections.Select (i => i == null).ToList ();

			// All null
			if (areCollectionsNull.All (isNull => isNull))
				return true;

			// Some null, some others not null
			if (areCollectionsNull.Any (isNull => isNull))
				return false;

			var collectionSize = collections [0].Count;
			if (collections.Any (item => item.Count != collectionSize))
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

		#if !SALTARELLE
		private static void EnsureIsNullable<T> ()
		{
			var type = typeof(T);

			if (type.IsPointer)
				return;

			if (Nullable.GetUnderlyingType (type) != null)
				return;

			if (type.IsByRef)
				return;

			throw new InvalidOperationException ("Type is not nullable or reference type.");
		}
		#endif
	}
}

