using System;
using UIKit;
using MasDev.iOS.Utils;
using Foundation;
using CoreGraphics;

namespace MasDev.iOS.Utils
{
	public static class UIImagePickerUtils
	{
		private static UIImagePickerController imagePicker;
		private static UIPopoverController popover;

		public static void PresentImagePickerController(UIViewController parent, UIView presentingView, Action<UIImage> onUIImageSelected)
		{
			imagePicker = new UIImagePickerController();
			imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
			imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes (UIImagePickerControllerSourceType.PhotoLibrary);

			imagePicker.FinishedPickingMedia += (sender, e) => Handle_FinishedPickingMedia (sender, e, onUIImageSelected);

			imagePicker.Canceled += Handle_Canceled;

			if(UIApplicationUtils.DeviceIsTablet) // We have to use a UIPopover for iPad
			{
				if(popover == null || popover.ContentViewController == null)
					popover = new UIPopoverController(imagePicker);

				popover.PresentFromRect(new CGRect(0, 0, 10, 10), presentingView, UIPopoverArrowDirection.Down, true);
			}
			else
			{
				parent.PresentViewController (imagePicker, true, null);
			}
		}

		private static void Handle_FinishedPickingMedia (object sender, UIImagePickerMediaPickedEventArgs e, Action<UIImage> onUIImageSelected)
		{
			// determine what was selected, video or image
			bool isImage = false;
			switch(e.Info[UIImagePickerController.MediaType].ToString()) {
			case "public.image":
				isImage = true;
				break;
			case "public.video":
				break;
			}

			// get common info (shared between images and video)
			NSUrl referenceURL = e.Info[new NSString("UIImagePickerControllerReferenceUrl")] as NSUrl;
			if (referenceURL != null)
				Console.WriteLine("Url:"+referenceURL.ToString ());

			// if it was an image, get the other image info
			if(isImage) {
				// get the original image
				UIImage originalImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;
				if(originalImage != null)
				{
					// do something with the image
					if (onUIImageSelected != null)
						onUIImageSelected.Invoke (originalImage);
				}
			}
			/*else { // if it's a video
				// get video url
				NSUrl mediaURL = e.Info[UIImagePickerController.MediaURL] as NSUrl;
				if(mediaURL != null) {
					Console.WriteLine(mediaURL.ToString());
				}
			}*/

			// dismiss the picker
			DismissImagePicker ();
		}

		private static void Handle_Canceled (object sender, EventArgs e)
		{
			DismissImagePicker ();
		}

		private static void DismissImagePicker()
		{
			imagePicker.DismissViewController(true, null);
			imagePicker.Dispose ();

			if (popover != null && popover.PopoverVisible)
				popover.Dismiss (true);
			popover = null;
		}
	}
}