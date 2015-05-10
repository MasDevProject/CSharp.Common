using System.Text.RegularExpressions;


namespace MasDev.Owin
{
	public static class PathUtils
	{
		const string _urlParameterRegexPattern = "{[^/]*?}";
		const string _urlParameterRegexReplacement = "[^/]*?";

		public static bool MatchesTemplate (this string path, string urlTemplate)
		{
			urlTemplate = Regex.Replace (urlTemplate, _urlParameterRegexPattern, _urlParameterRegexReplacement);

			var isTemplateMatched = Regex.IsMatch (path, urlTemplate);
			return isTemplateMatched;
		}
	}
}