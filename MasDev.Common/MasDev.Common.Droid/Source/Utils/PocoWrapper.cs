using System;

namespace MasDev.Droid.Utils
{
	public static class PocoWrapper 
	{
		public static PocoWrapper<T> Create<T> (Func<T, string> toString, T poco)
		{
			return new PocoWrapper<T> (toString, poco);
		}

		public static PocoWrapper<T> Create<T> (Func<T, string> toString)
		{
			return new PocoWrapper<T> (toString);
		}
	}

	public class PocoWrapper<T> : Java.Lang.Object
	{
		public T Poco { get; set; }
		readonly Func<T, string> _toString;

		public PocoWrapper (Func<T, string> toString)
		{
			_toString = toString;
		}

		public PocoWrapper (Func<T, string> toString, T poco) : this (toString)
		{
			Poco = poco;
		}

		public override string ToString ()
		{
			return _toString.Invoke (Poco); //poco
		}
	}
}

