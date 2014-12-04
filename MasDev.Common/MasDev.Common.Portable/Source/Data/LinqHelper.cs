using System;
using System.Linq.Expressions;
using MasDev.Common.Modeling;
using System.Reflection;
using System.Linq;


namespace MasDev.Common.Data
{
	public static class LinqHelper
	{
		public static Expression<Func<TValue, bool>> BuildOrChainExpression<TValue, TKey> (Func<ParameterExpression, AtomicExpressionBuilder<TValue, TKey>> atomicExpressionProvider, params TKey[] values)
		{
			if (values == null || values.Length == 0)
				throw new ArgumentException ();

			var parameterExpression = Expression.Parameter (typeof(TValue), "m");
			var builder = atomicExpressionProvider (parameterExpression);

			if (values.Length == 1)
				return Expression.Lambda<Func<TValue, bool>> (
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

			return Expression.Lambda<Func<TValue, bool>> (or, parameterExpression);
		}

		#region OrId

		public static Expression<Func<TModel, bool>> BuildOrIdExpression<TModel> (params int[] ids) where TModel : IModel
		{
			return BuildOrChainExpression (
				p => new IdEqualsExpressionBuilder<TModel> (p),
				ids
			);
		}



		class IdEqualsExpressionBuilder<TModel> : AtomicExpressionBuilder<TModel, int> where TModel: IModel
		{
			static readonly string _idPropertyName = typeof(IModel).GetRuntimeProperties ().Single ().Name;
			readonly MemberExpression _memberAccess;



			public IdEqualsExpressionBuilder (ParameterExpression parameterExpression) : base (parameterExpression)
			{
				_memberAccess = Expression.MakeMemberAccess (parameterExpression, typeof(TModel).GetRuntimeProperty (_idPropertyName));
			}



			public override Expression BuildAtomicExpression (int constantValue)
			{
				return Expression.Equal (_memberAccess, Expression.Constant (constantValue));
			}

		}

		#endregion
	}





	public abstract class AtomicExpressionBuilder<T, TKey>
	{
		public ParameterExpression ParameterExpression { get; private set; }



		protected AtomicExpressionBuilder (ParameterExpression parameterExpression)
		{
			ParameterExpression = parameterExpression;
		}



		public abstract Expression BuildAtomicExpression (TKey constantValue);

	}
}

