using System;
using UIKit;
using Foundation;

namespace MasDev.Common
{
	public static class UIDocumentControllerUtils
	{
		public static void PresentDocumentController (
			UIViewController presenter,
			string path,
			string fileName,
			bool animated,
			UIBarButtonItem btnShare = null,
			Action onPreviewFailAction = null)
		{
			var fileUrl = "file:" + Uri.EscapeUriString(path);

			var documentController = UIDocumentInteractionController.FromUrl(new NSUrl(fileUrl));
			documentController.Name = fileName;
			documentController.ViewControllerForPreview = (controller) => presenter;
			documentController.DidEndPreview += (s, e) => { documentController.Dispose(); documentController = null; };

			if (!documentController.PresentPreview (animated))
			if (btnShare != null && onPreviewFailAction != null && !documentController.PresentOpenInMenu (btnShare, animated))	
				onPreviewFailAction.Invoke();
		}
	}
}