using System;


namespace MasDev.Extensions
{
	public static class ArraysExtensions
	{
		public static bool IsEmpty (this object[] array)
		{
			return array.Length == 0;
		}

		public static T[] CopyUntil<T> (this T[] array, int count)
		{
			if (count > array.Length || count < 0)
				throw new ArgumentException ("Index out of bound");
			if (count == array.Length)
				return array;

			var arr = new T[count];
			for (int i = 0; i < count; i++)
				arr [i] = array [i];

			return arr;
		}
	}
}

