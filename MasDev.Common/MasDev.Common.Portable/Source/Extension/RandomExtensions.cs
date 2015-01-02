using System;


namespace MasDev.Extensions
{
	public static class RandomExtensions
	{
		public static bool NextBool (this Random r)
		{
			return r.NextBool (0.5f);
		}



		public static bool NextBool (this Random r, float successProbability)
		{
			return r.NextDouble () < successProbability;
		}



		public static decimal NextDecimal (this Random r, int min, int max)
		{
			var intPart = r.Next (min, max - 1);
			var floatPart = r.Next (0, 99) / (float)100;
			return Convert.ToDecimal (intPart + floatPart);
		}



		public static decimal? NextNullableDecimal (this Random r, int min, int max)
		{
			return r.NextBool () ? (decimal?)null : (decimal?)r.NextDecimal (min, max);

		}
	}
}

