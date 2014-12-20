using System;
using MasDev.Common.Modeling;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;

namespace MasDev.Common.Data.Expressions
{
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
}

