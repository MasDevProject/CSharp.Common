using System;
using System.Linq.Expressions;

namespace MasDev.Data.Expressions
{
	public static class ExpressionBuilder
	{
		public static Expression<Func<TSource, bool>> OrChain<TSource, TKey> (Func<ParameterExpression, AtomicExpressionBuilder<TSource, TKey>> atomicExpressionProvider, params TKey[] values)
		{
			if (values == null || values.Length == 0)
				throw new ArgumentException ();

			var parameterExpression = Expression.Parameter (typeof(TSource), "m");
			var builder = atomicExpressionProvider (parameterExpression);

			if (values.Length == 1)
				return Expression.Lambda<Func<TSource, bool>> (
					builder.BuildAtomicExpression (values [0]),
					parameterExpression
				);


			Expression or = null;
			for (int i = 0; i < values.Length; i++) {
				if (i == 1)
					continue;

				if (i != 0) {
					or = Expression.OrElse (or, builder.BuildAtomicExpression (values [i]));
					continue;
				}

				var atomic1 = builder.BuildAtomicExpression (values [i]);
				var atomic2 = builder.BuildAtomicExpression (values [i + 1]);
				or = Expression.OrElse (atomic1, atomic2);
			}

			while (or.CanReduce)
				or = or.Reduce ();

			return Expression.Lambda<Func<TSource, bool>> (or, parameterExpression);
		}


		public static Expression<Func<TModel, bool>> OrId<TModel> (params int[] ids) where TModel : IModel
		{
			return OrChain (
				p => new IdEqualsExpressionBuilder<TModel> (p),
				ids
			);
		}
	}
}

