using System;
using System.Linq;


namespace MasDev.Utils
{
	public static class EnumUtils
	{
		static readonly Random _random = new Random ();



		public static T Random<T> ()
		{
			var values = Enum.GetValues (typeof(T));
			return(T)values.GetValue (_random.Next (values.Length));
		}



		public static T[] Values<T> ()
		{
			return Enum.GetValues (typeof(T)).Cast<T> ().ToArray ();
		}



		public static T Parse<T> (string value)
		{
			return (T)(object)Enum.Parse (typeof(T), value);
		}

		public static int ValuesCount<T> ()
		{
			return Values<T> ().Length;
		}
	}
}

