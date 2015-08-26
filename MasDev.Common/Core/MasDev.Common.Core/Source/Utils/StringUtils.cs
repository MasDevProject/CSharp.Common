using System;
using System.Collections.Generic;
using MasDev.Extensions;
using System.Linq;

namespace MasDev.Utils
{
    public static class StringUtils
    {
        public const string Space = " ";
        public const char Comma = ',';

        public static string AsCsv(this IEnumerable<string> strings)
        {
            return string.Join(Comma.ToString(), strings);
        }

        public static string AsCsv<T>(this IEnumerable<T> strings, Func<T, string> toString)
        {
            return AsCsv(strings.Select(o => toString(o)));
        }

        public static void AddCsvValue(ref string s, string value)
        {
            if (s == null)
                return;

            var values = s.ReadCsv().ToList();
            if (values.Any(v => v == value))
                return;

            values.Add(value);
            s = values.AsCsv();
        }

        public static IEnumerable<string> ReadCsv(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return Enumerable.Empty<string>();

            return s.TrimEnd(Comma).Split(Comma);
        }

        public static int Length(string s)
        {
            return string.IsNullOrEmpty(s) ? 0 : s.Length;
        }

        public static bool ContainsSomethingReadable(string s)
        {
            return !(string.IsNullOrEmpty(s) || s.ContainsOnlyWhiteSpaces());
        }


#if !SALTARELLE
        public static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            var chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static string FromBase64(string encoded)
        {
            var bytes = Convert.FromBase64String(encoded);
            return GetString(bytes);
        }

        public static string[] PoorManStem(this string s, IEnumerable<string> stopWords)
        {
            var lowerString = s.ToLowerInvariant();
            lowerString = stopWords.Aggregate(lowerString, (current, stopWord) => current.Replace(stopWord, string.Empty));

            lowerString = lowerString.StripPunctuation();
            var words = lowerString.Trim().Split(' ');
            for (var i = 0; i < words.Length; i++)
                words[i] = words[i].Trim();

            return words;
        }
#endif
    }
}

