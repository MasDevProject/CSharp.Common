using System;
using System.Linq;
using System.Linq.Expressions;

namespace MasDev.Common.Data.Expressions
{
	public static class IQueryableExtensions
	{
		public static IQueryable<T> WhereContainsAllStrings<T> (this IQueryable<T> queryable, Expression<Func<T, string>> propertyExpression, params string[] strings)
		{
			return queryable.Where (FullExpressionBulder.StringContains (propertyExpression, strings));
		}

		public static IQueryable<T> WhereContainsAllStrings<T> (this IQueryable<T> queryable, string propertyName, params string[] strings)
		{
			return queryable.Where (FullExpressionBulder.StringContains<T> (propertyName, strings));
		}
	}
}

