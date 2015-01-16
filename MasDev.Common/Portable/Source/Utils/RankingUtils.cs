using System;
using System.Linq.Expressions;


namespace MasDev.Utils
{
	public static class RankingUtils
	{
		/// <summary>
		/// Normalizes the rank.
		/// </summary>
		/// <returns>The normalized rank.</returns>
		/// <param name="rank">The rank to be normalized</param>
		/// <param name="rankCount">Rank count.</param>
		/// <param name="rankAverage">The average rank of all items</param>
		/// <param name="minRankCount">Minimum rank count to be listed.</param>
		public static float NormalizeRank (float rank, float rankCount, float rankAverage, float minRankCount = 1f)
		{
			var m = minRankCount;
			var r = rank;
			var v = rankCount;
			var c = rankAverage;

			return (v / (v + m)) * r + (m / (v + m)) * c;
		}



		public static decimal NormalizeRank (decimal rank, decimal rankCount, decimal rankAverage, decimal minRankCount = 1m)
		{
			var m = minRankCount;
			var r = rank;
			var v = rankCount;
			var c = rankAverage;

			return (v / (v + m)) * r + (m / (v + m)) * c;
		}



		public static Expression<Func<double, double, double, double>> GetNormalizeRankExpression ()
		{
			return (rank, rankCount, rankAverage) => (rankCount / (rankCount + 1d)) * rank + (1d / (rankCount + 1d)) * rankAverage;
		}
	}
}

