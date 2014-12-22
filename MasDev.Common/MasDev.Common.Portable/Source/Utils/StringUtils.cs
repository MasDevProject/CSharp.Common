using System;
using System.Collections.Generic;
using System.Text;
using MasDev.Common.Extensions;
using System.Linq;


namespace MasDev.Common
{
	public static class StringUtils
	{
		public const string Space = " ";

		public static byte[] GetBytes (string str)
		{
			var bytes = new byte[str.Length * sizeof(char)];
			Buffer.BlockCopy (str.ToCharArray (), 0, bytes, 0, bytes.Length);
			return bytes;
		}



		public static string GetString (byte[] bytes)
		{
			var chars = new char[bytes.Length / sizeof(char)];
			Buffer.BlockCopy (bytes, 0, chars, 0, bytes.Length);
			return new string (chars);
		}



		public static string FromBase64 (string encoded)
		{
			var bytes = Convert.FromBase64String (encoded);
			return GetString (bytes);
		}

	
		public static string CommaSeparate (IEnumerable<string> strings)
		{
			if (strings == null)
				return null;

			var builder = new StringBuilder ();
			foreach (var s in strings)
				builder.Append (s).Append (',');
			return builder.ToString ();
		}

		public static string CommaSeparate (IEnumerable<object> strings)
		{
			return CommaSeparate (strings.Select (o => o.ToString ()));
		}

		public static string CommaSeparate<T> (IEnumerable<T> strings, Func<T, string> toString)
		{
			return CommaSeparate (strings.Select (o => toString(o)));
		}

		public static void AddCommaSeparatedValue (ref string s, string value)
		{
			if (s == null)
				return;

			var values = s.ReadAsCommaSeparatedValues ();
			if (values.Any (v => v == value))
				return;

			s = s + value + ',';
		}

		public static int Length (string s)
		{
			return string.IsNullOrEmpty (s) ? 0 : s.Length;
		}

		public static bool ContainsSomethingReadable (string s)
		{
			return !(string.IsNullOrEmpty (s) || s.ContainsOnlyWhiteSpaces ());
		}

		public static string[] PoorManStem (this string s, IEnumerable<string> stopWords)
		{
			var lowerString = s.ToLowerInvariant ();
			lowerString = stopWords.Aggregate (lowerString, (current, stopWord) => current.Replace (stopWord, string.Empty));

			lowerString = lowerString.StripPunctuation ();
			var words = lowerString.Trim ().Split (' ');
			for (var i = 0; i < words.Length; i++)
				words [i] = words [i].Trim ();

			return words;
		}
	}
}

