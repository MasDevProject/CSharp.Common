using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;


namespace MasDev.Common.Extensions
{
	public static class StringExtensions
	{
		const string JsonMime = "application/json";
		const string Http = "http:\\";
		const string Https = "https:\\";



		public static Uri AsUri (this string s)
		{
			return new Uri (s);
		}



		public static bool ContainsIgnoreCase (this string s, string what)
		{
			return s.ToLower ().Contains (what.ToLower ());
		}



		public static bool EqualsIgnoreCase (this string s, string what)
		{
			return string.CompareOrdinal (s, what) == 0;
		}



		public static bool ContainsOnlyWhiteSpaces (this string s)
		{
			return s.Trim ().Length == 0;
		}



		public static HttpContent AsJsonHttpContent (this string s)
		{
			return new StringContent (s, Encoding.UTF8, JsonMime);
		}



		public static int AsInt (this string s)
		{
			return int.Parse (s);
		}



		public static void Append (this StringBuilder b, string s, params object[] args)
		{
			b.Append (string.Format (s, args));
		}



		public static byte[] AsByteArray (this string s)
		{
			return StringUtils.GetBytes (s);
		}



		public static bool IsLocalPath (this string path)
		{
			try
			{
				if (path.StartsWith (Http, StringComparison.Ordinal) || path.StartsWith (Https, StringComparison.Ordinal))
					return false;

				return new Uri (path).IsFile;
			} catch (Exception)
			{
				return true;
			}
		}



		public static string BetweenExclusive (this string s, string lowerBound, string upperBound)
		{
			var groups = Regex.Match (s, lowerBound + @"(.+?)" + upperBound).Groups;
			if (groups == null || groups.Count == 0)
				return null;


			var match = groups [1];
			return match == null ? null : match.Value;
		}



		public static string AfterExclusive (this string s, string what)
		{
			var index = s.IndexOf (what, StringComparison.Ordinal);
			return index < 0 ? null : s.Substring (index + what.Length);
		}



		public static string StripPunctuation (this string s)
		{
			return new string (StripPunctuationInt (s).ToArray ());
		}



		static IEnumerable<char> StripPunctuationInt (string s)
		{
			for (int i = 0; i < s.Length; i++)
			{
				var c = s [i];
				if (!char.IsPunctuation (c))
					yield return c;
			}
		}

		public static string CollapseMultipleSpaces (this string s) 
		{
			return Regex.Replace (s, @"\s+", StringUtils.Space);
		}
	}
}

