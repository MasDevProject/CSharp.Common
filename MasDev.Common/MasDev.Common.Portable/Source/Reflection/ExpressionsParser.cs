using System;
using System.Linq.Expressions;
using System.Reflection;


namespace MasDev.Common.Reflection
{
	public static class ExpressionsParser
	{
		public static string DynamicParsePropertyName (dynamic propertyLambda)
		{
			return DynamicParsePropertyInfo (propertyLambda).Name;
		}



		public static string ParsePropertyName<TSource,TKey> (Expression<Func<TSource, TKey>> propertyLambda)
		{
			return ParsePropertyInfo (propertyLambda).Name;
		}



		public static PropertyInfo ParsePropertyInfo<TSource,TKey> (Expression<Func<TSource, TKey>> propertyLambda)
		{
			//var type = typeof(TSource);

			var member = propertyLambda.Body as MemberExpression;
			if (member == null)
				throw new ArgumentException (string.Format ("Expression '{0}' refers to a method, not a property.", propertyLambda));

			var propInfo = member.Member as PropertyInfo;

			if (propInfo == null)
				throw new ArgumentException (string.Format ("Expression '{0}' refers to a field, not a property.", propertyLambda));

			//			if (type != propInfo.ReflectedType && !type.IsSubclassOf (propInfo.ReflectedType))
			//				throw new ArgumentException (string.Format ("Expresion '{0}' refers to a property that is not from type {1}.", propertyLambda, type));

			return propInfo;
		}



		public static PropertyInfo DynamicParsePropertyInfo (dynamic propertyLambda)
		{
			var member = propertyLambda as MemberExpression ?? propertyLambda.Body as MemberExpression;

			if (member == null)
				throw new ArgumentException (string.Format ("Expression '{0}' refers to a method, not a property.", propertyLambda));

			var propInfo = member.Member as PropertyInfo;

			if (propInfo == null)
				throw new ArgumentException (string.Format ("Expression '{0}' refers to a field, not a property.", propertyLambda));

			//			if (type != propInfo.ReflectedType && !type.IsSubclassOf (propInfo.ReflectedType))
			//				throw new ArgumentException (string.Format ("Expresion '{0}' refers to a property that is not from type {1}.", propertyLambda, type));

			return propInfo;
		}
	}
}

