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
			byte[] bytes = new byte[str.Length * sizeof(char)];
			System.Buffer.BlockCopy (str.ToCharArray (), 0, bytes, 0, bytes.Length);
			return bytes;
		}



		public static string GetString (byte[] bytes)
		{
			char[] chars = new char[bytes.Length / sizeof(char)];
			System.Buffer.BlockCopy (bytes, 0, chars, 0, bytes.Length);
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
			if (string.IsNullOrEmpty (s))
				return 0;
			return s.Length;
		}
	}
}

