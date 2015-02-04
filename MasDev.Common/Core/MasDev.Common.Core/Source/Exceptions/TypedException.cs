using System;


namespace MasDev.Exceptions
{
	public class TypedException<T> : Exception 
	{
		public T Cause { get; private set; }

		internal TypedException (T exceptionEnum) : base (exceptionEnum.ToString ())
		{
			Cause = exceptionEnum;
		}
	}

	public static class TypedException
	{
		public static TypedException<T> Create<T> (T exceptionEnum)
		{
			return new TypedException<T> (exceptionEnum);
		}
	}
}

