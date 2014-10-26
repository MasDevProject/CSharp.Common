using System;


namespace MasDev.Common.Reflection
{
	public static class Types
	{
		public static bool IsNativeType<T> ()
		{
			return IsNativeType (typeof(T));
		}



		public static bool IsNativeType (object value)
		{
			return IsNativeType (value.GetType ());
		}



		public static bool IsNativeType (Type type)
		{
			return 
				type == typeof(double) || type == typeof(Double) ||
			type == typeof(float) ||
			type == typeof(decimal) || type == typeof(Decimal) ||
			type == typeof(int) || type == typeof(Int16) || type == typeof(Int32) || type == typeof(Int64) ||
			type == typeof(long) ||
			type == typeof(char) || type == typeof(Char) ||
			type == typeof(byte) || type == typeof(Byte) ||
			type == typeof(string) || type == typeof(String);
		}



		public static bool IsRealNumber<T> ()
		{
			return IsRealNumber (typeof(T));
		}



		public static bool IsRealNumber (Type type)
		{
			return 
				type == typeof(double) || type == typeof(Double) ||
			type == typeof(float) ||
			type == typeof(decimal) || type == typeof(Decimal) ||
			type == typeof(int) || type == typeof(Int16) || type == typeof(Int32) || type == typeof(Int64) ||
			type == typeof(long);
		}
	}
}

