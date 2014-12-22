using System;

namespace MasDev.Common.Utils
{
	public static class DelegateUtils
	{
		public static readonly Action Void = () => {};

		public static Action<T> TypedVoid<T> ()
		{
			return t => {};
		}
	}
}

