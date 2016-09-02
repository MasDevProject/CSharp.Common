using System;
using UIKit;
using System.IO;
using Foundation;

namespace MasDev.Common
{
	public static class UIWebViewExtenions
	{
		public static void UpdateContent(this UIWebView webView, string htmlContent, bool allowEmptyContent = false)
		{
			if (!allowEmptyContent && string.IsNullOrWhiteSpace (htmlContent))
				return;

			if (allowEmptyContent && htmlContent == null)
				return;

			// assumes all files are places in the Content folder
			var contentDirectoryPath = Path.Combine (NSBundle.MainBundle.BundlePath, "Content/");
			webView.LoadHtmlString(htmlContent, new NSUrl(contentDirectoryPath, true));
		}
	}
}