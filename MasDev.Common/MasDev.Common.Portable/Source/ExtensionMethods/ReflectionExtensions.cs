using System.Reflection;
using System.Runtime.CompilerServices;


namespace MasDev.Common.Extensions
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
	}
}

