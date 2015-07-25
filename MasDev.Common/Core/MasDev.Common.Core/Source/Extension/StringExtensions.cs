using System;
using System.Text;
using System.Collections.Generic;
using MasDev.Utils;

#if !SALTARELLE
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Linq;
#endif


namespace MasDev.Extensions
{
	public static class StringExtensions
	{
		const string JsonMime = "application/json";
		const string Http = "http:\\";
		const string Https = "https:\\";

		public static bool EqualsIgnoreCase (this string s, string what)
		{
			if (Check.BothNull (s, what))
				return true;

			if (!Check.BothNotNull (s, what))
				return false;

			if (s.Length != what.Length)
				return false;


			return s.ToLower () == what.ToLower ();
		}

		public static bool ContainsIgnoreCase (this string s, string what)
		{
			return s.ToLower ().Contains (what.ToLower ());
		}

		public static bool ContainsOnlyWhiteSpaces (this string s)
		{
			return s.Trim ().Length == 0;
		}

		public static int AsInt (this string s)
		{
			return int.Parse (s);
		}

		public static void Append (this StringBuilder b, string s, params object[] args)
		{
			b.Append (string.Format (s, args));
		}

		public static string BetweenExclusive (this string s, string lowerBound, string upperBound)
		{
			var startIndex = s.IndexOf (lowerBound);
			var actualStart = startIndex + lowerBound.Length;
			var endIndex = s.LastIndexOf (upperBound);

			if (startIndex == endIndex)
				return string.Empty;

			if (startIndex < 0 || endIndex < 0 || endIndex < actualStart)
				return s;

			return s.Substring (actualStart, endIndex - actualStart);
		}

		public static string AfterExclusive (this string s, string what)
		{
			var index = s.IndexOf (what);
			return index < 0 ? null : s.Substring (index + what.Length);
		}

		public static int Occurrences (this string s, string value)
		{                  
			int count = 0, minIndex = s.IndexOf (value, 0);
			while (minIndex != -1) {
				minIndex = s.IndexOf (value, minIndex + value.Length);
				count++;
			}
			return count;
		}

		public static IEnumerable<string> ReadAsCommaSeparatedValues (this string s)
		{
            if (s == null)
                return Enumerable.Empty<string>();
            return s.Split(',').Where(r => !string.IsNullOrWhiteSpace(r));
		}

		public static string Before (this string s, char c)
		{
			var index = s.IndexOf (c);
			return index <= 0 ? s : s.Substring (0, index);
		}

		public static string BeforeLast (this string s, char c)
		{
			var index = s.LastIndexOf (c);
			return index <= 0 ? s : s.Substring (0, index);
		}

		public static bool IsHttpPath (this string s)
		{
			try {
				Uri uriResult;
				return  Uri.TryCreate (s, UriKind.Absolute, out uriResult) && (uriResult.Scheme == "http" || uriResult.Scheme == "https");
			} catch {
				return false;
			}
		}

		public static bool Matches (this string s, string regex)
		{
			var match = Regex.Match (s, regex);
			return match.Success && match.Groups != null && match.Groups.Count > 0;
		}

		#if !SALTARELLE
		public static Uri AsUri (this string s)
		{
			return new Uri (s);
		}

		public static string CollapseMultipleSpaces (this string s)
		{
			return Regex.Replace (s, @"\s+", StringUtils.Space);
		}

		static IEnumerable<char> StripPunctuationInt (string s)
		{
			for (int i = 0; i < s.Length; i++) {
				var c = s [i];
				if (!char.IsPunctuation (c))
					yield return c;
			}
		}

		public static string StripPunctuation (this string s)
		{
			return new string (StripPunctuationInt (s).ToArray ());
		}

		public static bool IsLocalPath (this string path)
		{
			try {
				if (path.StartsWith (Http, StringComparison.Ordinal) || path.StartsWith (Https, StringComparison.Ordinal))
					return false;

				return new Uri (path).IsFile;
			} catch (Exception) {
				return true;
			}
		}

		public static HttpContent AsJsonHttpContent (this string s)
		{
			return new StringContent (s, Encoding.UTF8, JsonMime);
		}

		public static byte[] AsByteArray (this string s)
		{
			return StringUtils.GetBytes (s);
		}
		#endif
	}
}

