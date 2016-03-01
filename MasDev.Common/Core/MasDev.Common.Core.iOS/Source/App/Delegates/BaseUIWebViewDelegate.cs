using System;
using UIKit;
using CoreGraphics;

namespace MasDev.Common
{
	public class BaseUIWebViewDelegate : UIWebViewDelegate
	{
		protected virtual float MinHeight { get; } = 10;

		public event Action<CGSize> OnSizeChanged;

		public override void LoadingFinished (UIWebView webView)
		{
			if (OnSizeChanged != null)
				OnSizeChanged.Invoke (webView.ScrollView.ContentSize);
		}
	}
}