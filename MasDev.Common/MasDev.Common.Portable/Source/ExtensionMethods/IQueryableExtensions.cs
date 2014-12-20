using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace MasDev.Common.Extensions
{
	public static class IQueryableExtensions
	{
		public static Task<bool> AllAsync<T> (this IQueryable<T> queryable, Expression<Func<T,bool>> predicate)
		{
			return Task.FromResult (queryable.All (predicate));
		}



		public static Task<bool> AnyAsync<T> (this IQueryable<T> queryable, Expression<Func<T,bool>> predicate)
		{
			return Task.FromResult (queryable.Any (predicate));
		}



		public static Task<bool> AnyAsync<T> (this IQueryable<T> queryable)
		{
			return Task.FromResult (queryable.Any ());
		}



		public static Task<double> AverageAsync<T> (this IQueryable<T> queryable, Expression<Func<T,long>> selector)
		{
			return Task.FromResult (queryable.Average (selector));
		}



		public static Task<bool> ContainsAsync<T> (this IQueryable<T> queryable, T item)
		{
			return Task.FromResult (queryable.Contains (item));
		}



		public static Task<int> CountAsync<T> (this IQueryable<T> queryable, Expression<Func<T,bool>> predicate)
		{
			return Task.FromResult (queryable.Count (predicate));
		}



		public static Task<int> CountAsync<T> (this IQueryable<T> queryable)
		{
			return Task.FromResult (queryable.Count ());
		}



		public static Task<T> ElementAtAsync<T> (this IQueryable<T> queryable, int index)
		{
			return Task.FromResult (queryable.ElementAt (index));
		}



		public static Task<T> ElementAtOrDefaultAsync<T> (this IQueryable<T> queryable, int index)
		{
			return Task.FromResult (queryable.ElementAtOrDefault (index));
		}



		public static Task<T> FirstAsync<T> (this IQueryable<T> queryable, Expression<Func<T,bool>> predicate)
		{
			return Task.FromResult (queryable.First (predicate));
		}



		public static Task<T> FirstAsync<T> (this IQueryable<T> queryable)
		{
			return Task.FromResult (queryable.First ());
		}



		public static Task<T> FirstOrDefaultAsync<T> (this IQueryable<T> queryable, Expression<Func<T,bool>> predicate)
		{
			return Task.FromResult (queryable.FirstOrDefault (predicate));
		}



		public static Task<T> FirstOrDefaultAsync<T> (this IQueryable<T> queryable)
		{
			return Task.FromResult (queryable.FirstOrDefault ());
		}



		public static Task<T> LastAsync<T> (this IQueryable<T> queryable, Expression<Func<T,bool>> predicate)
		{
			return Task.FromResult (queryable.Last (predicate));
		}



		public static Task<T> LastAsync<T> (this IQueryable<T> queryable)
		{
			return Task.FromResult (queryable.Last ());
		}



		public static Task<T> LastOrDefaultAsync<T> (this IQueryable<T> queryable, Expression<Func<T,bool>> predicate)
		{
			return Task.FromResult (queryable.LastOrDefault (predicate));
		}



		public static Task<T> LastOrDefaultAsync<T> (this IQueryable<T> queryable)
		{
			return Task.FromResult (queryable.LastOrDefault ());
		}



		public static Task<TResult> MaxAsync<T, TResult> (this IQueryable<T> queryable, Expression<Func<T,TResult>> predicate)
		{
			return Task.FromResult (queryable.Max (predicate));
		}



		public static Task<T> MaxAsync<T> (this IQueryable<T> queryable)
		{
			return Task.FromResult (queryable.Max ());
		}



		public static Task<TResult> MinAsync<T, TResult> (this IQueryable<T> queryable, Expression<Func<T,TResult>> predicate)
		{
			return Task.FromResult (queryable.Min (predicate));
		}



		public static Task<T> MinAsync<T> (this IQueryable<T> queryable)
		{
			return Task.FromResult (queryable.Min ());
		}



		public static Task<T> SingleAsync<T> (this IQueryable<T> queryable, Expression<Func<T,bool>> predicate)
		{
			return Task.FromResult (queryable.Single (predicate));
		}



		public static Task<T> SingleAsync<T> (this IQueryable<T> queryable)
		{
			return Task.FromResult (queryable.Single ());
		}



		public static Task<T> SingleOrDefaultAsync<T> (this IQueryable<T> queryable, Expression<Func<T,bool>> predicate)
		{
			return Task.FromResult (queryable.SingleOrDefault (predicate));
		}



		public static Task<T> SingleOrDefaultAsync<T> (this IQueryable<T> queryable)
		{
			return Task.FromResult (queryable.SingleOrDefault ());
		}



		public static Task<long> SumAsync<T> (this IQueryable<T> queryable, Expression<Func<T,long>> predicate)
		{
			return Task.FromResult (queryable.Sum (predicate));
		}



		public static Task<T[]> ToArrayAsync<T> (this IQueryable<T> queryable)
		{
			return Task.FromResult (queryable.ToArray ());
		}



		public static Task<List<T>> ToListAsync<T> (this IQueryable<T> queryable)
		{
			return Task.FromResult (queryable.ToList ());
		}



		public static int FindFirstIndex<T> (this IEnumerable<T> enumerable, Predicate<T> predicate)
		{
			int i = 0;
			foreach (var t in enumerable) {
				if (predicate (t))
					return i;
				i++;
			}
			return -1;
		}
	}
}

