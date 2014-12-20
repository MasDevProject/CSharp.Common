using System;
using System.Linq.Expressions;

namespace MasDev.Common.Data.Expressions
{
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

