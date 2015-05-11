using System.Text.RegularExpressions;
using Microsoft.Owin;


namespace MasDev.Common.Owin
{
	public static class PathUtils
	{
		const string _normalizeSlashesRegexPattern = @"/+";
		const string _normalizeSlashesRegexReplacement = "/";
		const string _urlParameterRegexPattern = @"{[^/]*?}";
		const string _urlParameterRegexReplacement = @"[^/]*?";
		static readonly char[] _trimEndCharacters = { '\\', '/' };

		public static bool MatchesTemplate (this string path, string urlTemplate)
		{
			urlTemplate = Regex.Replace (urlTemplate, _urlParameterRegexPattern, _urlParameterRegexReplacement);

			var isTemplateMatched = Regex.IsMatch (path, urlTemplate);
			return isTemplateMatched;
		}

		public static string ToNormalizedString (this PathString path)
		{
			return Regex.Replace (path.ToString (), _normalizeSlashesRegexPattern, _normalizeSlashesRegexReplacement).TrimEnd (_trimEndCharacters);
		}
	}
}