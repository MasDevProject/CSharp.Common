using System;


namespace MasDev.Common.Reflection
{
	public interface IType<out T>
	{
		Type Type { get; }
	}





	public static class TypeEx
	{
		public static IType<T> GetType<T> ()
		{
			return new Type<T> (typeof(T));
		}
	}





	class Type<T> : IType<T>
	{
		Type t;



		internal Type (Type t)
		{
			this.t = t;
		}



		Type IType<T>.Type
		{
			get {
				return t;
			}
		}
	}
}

