using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using System;
using MasDev.Reflection;


namespace MasDev.Extensions
{
	public static class ReflectionExtensions
	{
		public static bool IsAsync (this MethodInfo info)
		{
			return info.GetCustomAttribute (typeof(AsyncStateMachineAttribute)) != null;
		}



		public static T Cast<T> (this object o)
		{
			return (T)o;
		}



		public static string ResolvePropertyName<TSource, TKey> (this Expression<Func<TSource, TKey>> propertyExpression)
		{
			return ExpressionsParser.ParsePropertyName (propertyExpression);
		}
	}
}

