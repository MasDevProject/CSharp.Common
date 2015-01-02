using System;
using System.Linq;
using System.Linq.Expressions;
using MasDev.Data.Expressions;

namespace MasDev.Data
{
	public static class IQueryableExpressionExtensions
	{
		public static IQueryable<T> WhereContainsSomeStrings<T> (this IQueryable<T> queryable, Expression<Func<T, string>> propertyExpression, params string[] strings)
		{
			return queryable.Where (FullExpressionBulder.StringContains (propertyExpression, strings));
		}

		public static IQueryable<T> WhereContainsSomeStrings<T> (this IQueryable<T> queryable, string propertyName, params string[] strings)
		{
			return queryable.Where (FullExpressionBulder.StringContains<T> (propertyName, strings));
		}
	}
}

