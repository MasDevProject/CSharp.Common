using MasDev.Common.Extensions;
using System;

namespace MasDev.Common.Utils
{
	public static class BitwiseUtils
	{
		public static bool Has (int sourceFlag, int destFlag)
		{
			return (sourceFlag & destFlag) == destFlag;
		}



		public static bool HasExcactly (int sourceFlag, params int[] flags)
		{
			Assert.NotNullOrEmpty (flags);
			foreach (var flag in flags)
				if (sourceFlag == flag)
					return true; 
			return false;
		}


		public static T BitwiseRemove<T> (this T source, T bitsToRemove)
		{
			var s = source.Cast<int> ();
			var r = bitsToRemove.Cast<int> ();
			var n = BitwiseRemove (s, r);

			return n.Cast<T> ();
		}


		public static int BitwiseRemove (this int source, int bitsToRemove)
		{
			var sourceBits = source.ToBooleanArray ();
			var removeBits = bitsToRemove.ToBooleanArray ();

			var index = sourceBits.Length;
			for (var i = removeBits.Length - 1; i >= 0; i--) {
				index--;
				var current = removeBits [i];
				if (!current)
					continue;

				sourceBits [index] = false;
			}

			return sourceBits.ToInt ();
		}


		public static int ToInt (this bool[] bitArray)
		{
			var value = 0;
			for (var i = bitArray.Length - 1; i >= 0; i--) {
				var current = bitArray [i] ? 1 : 0;
				value += (int)(current * Math.Pow (2, (bitArray.Length - 1) - i));
			}
			return value;
		}
	}
}
