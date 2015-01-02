using System;
using System.Linq;

namespace MasDev.Extensions
{
	public static class NumberExtensions
	{
		public static bool[] ToBooleanArray (this int i)
		{
			return Convert.ToString (i, 2).ToCharArray ().Select (s => s == '1').ToArray ();
		}

		public static int[] ToBitArray (this int i)
		{
			return Convert.ToString (i, 2).ToCharArray ().Select (s => s == '1' ? 1 : 0).ToArray ();
		}
	}
}

