using System.Text.RegularExpressions;


namespace MasDev.Services
{
	public static class PathUtils
	{
		const string _delimiterFormat = "^/{0}$";
		const string _normalizeSlashesRegexPattern = @"/+";
		const string _normalizeSlashesRegexReplacement = "/";
		const string _urlParameterRegexPattern = @"{[^/]+?}";
		const string _urlParameterRegexReplacement = @"[^/]+?";
		static readonly char[] _trimEndCharacters = { '\\', '/' };

		public static bool MatchesTemplate (this string path, string urlTemplate)
		{
			urlTemplate = Regex.Replace (urlTemplate, _urlParameterRegexPattern, _urlParameterRegexReplacement);
			urlTemplate = string.Format (_delimiterFormat, urlTemplate).NormalizePath ();

			var isTemplateMatched = Regex.IsMatch (path.NormalizePath (), urlTemplate);
			return isTemplateMatched;
		}

		public static string NormalizePath (this string path)
		{
			return Regex.Replace (path, _normalizeSlashesRegexPattern, _normalizeSlashesRegexReplacement).TrimEnd (_trimEndCharacters);
		}
	}
}