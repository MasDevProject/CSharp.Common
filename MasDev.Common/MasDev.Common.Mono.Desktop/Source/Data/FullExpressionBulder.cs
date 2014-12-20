using System;
using System.Linq.Expressions;

namespace MasDev.Common.Data.Expressions
{
	public static class FullExpressionBulder
	{
		public static Expression<Func<TSource, bool>> StringContains<TSource> (Expression<Func<TSource, string>> propertyExpression, params string[] strings)
		{
			return ExpressionBuilder.OrChain (
				p => StringContainsExpressionBuilder.Create (propertyExpression, p),
				strings
			);
		}


		public static Expression<Func<TSource, bool>> StringContains<TSource> (string propertyName, params string[] strings)
		{
			return ExpressionBuilder.OrChain (
				p => StringContainsExpressionBuilder.Create<TSource> (propertyName, p),
				strings
			);
		}

	}
}

