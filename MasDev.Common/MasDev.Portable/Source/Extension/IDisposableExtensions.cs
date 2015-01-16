using System;


namespace MasDev.Extensions
{
	public static class IDisposableExtensions
	{
		public static void DisposeIfNotNull (this IDisposable disposable)
		{
			if (disposable != null)
				disposable.Dispose ();
		}



		public static void Dispose<T> (this Lazy<T> lazy) where T : IDisposable
		{
			if (lazy.IsValueCreated)
				lazy.Value.DisposeIfNotNull ();
		}
	}
}

