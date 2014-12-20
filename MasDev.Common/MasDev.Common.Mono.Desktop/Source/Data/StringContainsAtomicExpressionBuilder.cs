using System;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using MasDev.Common.Extensions;

namespace MasDev.Common.Data.Expressions
{
	public class StringContainsExpressionBuilder<T> : AtomicExpressionBuilder<T, string>
	{
		//REMEMBER occhio quando si rinomina la variabile
		static readonly MethodInfo _toLowerMethod = typeof(string).GetMethods ().First (m => m.Name == "ToLower" && m.GetParameters ().Length == 0);
		static readonly MethodInfo _containsMethod = typeof(string).GetMethods ().First (m => m.Name == "Contains" && m.GetParameters ().Length == 1 && m.GetParameters () [0].ParameterType == typeof(string));
		readonly MethodCallExpression _propertyToLowerExpression;
		readonly string _propertyName;


		public StringContainsExpressionBuilder (ParameterExpression parameterExpression, string propertyName) : base (parameterExpression)
		{
			_propertyName = propertyName;
			var memberAccess = Expression.MakeMemberAccess (parameterExpression, typeof(T).GetRuntimeProperty (_propertyName));
			_propertyToLowerExpression = Expression.Call (memberAccess, _toLowerMethod);
		}



		public override Expression BuildAtomicExpression (string constantValue)
		{
			return Expression.Call (
				_propertyToLowerExpression,
				_containsMethod, 
				new []{ Expression.Constant (constantValue) }
			);
		}
	}


	static class StringContainsExpressionBuilder
	{
		public static StringContainsExpressionBuilder<T> Create<T> (string propertyName, ParameterExpression parameterExpression)
		{
			return new StringContainsExpressionBuilder<T> (parameterExpression, propertyName);
		}


		public static StringContainsExpressionBuilder<T> Create<T> (Expression<Func<T, string>> propertyExpression, ParameterExpression parameterExpression)
		{
			var propertyName = propertyExpression.ResolvePropertyName ();
			return new StringContainsExpressionBuilder<T> (parameterExpression, propertyName);
		}
	}
}

