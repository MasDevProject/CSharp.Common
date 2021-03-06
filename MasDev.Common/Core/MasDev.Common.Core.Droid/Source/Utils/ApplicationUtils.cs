﻿using System;
using Android.Content;
using Android.Content.Res;
using Android.App;
using Android.Views.InputMethods;
using Android.Util;
using Android.Provider;
using Android.Graphics;
using Android.Views;
using Android.Content.PM;
using Android.Media;

namespace MasDev.Droid.Utils
{
	public delegate void IntentStartFailedDelegate (Context ctx, Exception e);

	public static class ApplicationUtils
	{
		public static Context Context { get { return Application.Context; }}

		public static Point ScreenSize (Activity activity)
		{
			var display = activity.WindowManager.DefaultDisplay;
			var size = new Point ();
			display.GetSize (size);
			return size;
		}

		static int _deviceIsTablet = -1;
		public static bool DeviceIsTablet
		{
			get {
				if (_deviceIsTablet == -1) {
					var x = Context.Resources.Configuration.ScreenLayout & ScreenLayout.SizeMask;
					_deviceIsTablet = ((x == ScreenLayout.SizeLarge) || (x == ScreenLayout.SizeXlarge)) ? 1 : 0;
				}
				return _deviceIsTablet == 1;
			}
		}

		public static PackageInfo PackageInfo
		{
			get {
				return Context.PackageManager.GetPackageInfo (Context.PackageName, 0);
			}
		}

		public static string ApplicationFolderPath
		{
			get {
				return Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			}
		}

		public static decimal ScreenSizeInch 
		{
			get {
				var dm = Context.Resources.DisplayMetrics;

				double density = dm.Density * 160;
				double x = Math.Pow (dm.WidthPixels / density, 2);
				double y = Math.Pow (dm.HeightPixels / density, 2);
				return (decimal)Math.Sqrt (x + y);
			}
		}

		public static void IndexFile (Context ctx, string filePath, string mimeType, MediaScannerConnection.IOnScanCompletedListener callback = null)
		{
			MediaScannerConnection.ScanFile (ctx, new [] { filePath }, new [] { mimeType }, callback);
		}

		public static void HideKeyboard (Activity context)
		{
			try {
				((InputMethodManager)context.GetSystemService (Context.InputMethodService)).HideSoftInputFromWindow (context.Window.DecorView.WindowToken, HideSoftInputFlags.NotAlways);
			}
			catch (Exception e) 
			{
				Log.Debug ("ApplicationUtils", "showkeyboard: " + e); 
			}
		}

		public static void ShowKeyboard (Activity context)
		{
			try {
				((InputMethodManager)context.GetSystemService (Context.InputMethodService)).ToggleSoftInput (ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
			} 
			catch (Exception e) 
			{
				Log.Debug ("ApplicationUtils", "showkeyboard: " + e); 
			}
		}

		public static void CopyStringToClipBoard (string value)
		{
			var clipboard = (ClipboardManager)Context.GetSystemService (Context.ClipboardService);
			var clip = ClipData.NewPlainText (string.Empty, value);
			clipboard.PrimaryClip = clip;
		}

		public static int ConvertDpToPixel (Context ctx, float dp)
		{
			return (int)TypedValue.ApplyDimension (ComplexUnitType.Dip, dp, ctx.Resources.DisplayMetrics);
		}
			
		public static class Intents
		{
			public static void OpenFile (Context ctx, string filePath, string mimeType, IntentStartFailedDelegate onError = null)
			{
				try {
					var intent = new Intent ();
					intent.SetAction (Intent.ActionView);
					intent.SetDataAndType (Android.Net.Uri.Parse("file://" + filePath), mimeType);
					ctx.StartActivity (intent);
				} catch (Exception e) {
					onError?.Invoke (ctx, e);
				}
			}

			public static void AddCloseAllActivitiesFlag (Intent intent)
			{
				intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
			}

			public static void StartSettingsActivity (Activity ctx)
			{
				ctx.StartActivityForResult (new Intent (Settings.ActionSettings), 0);
			}

			public static void StartBrowserActivity (Context ctx, string url, IntentStartFailedDelegate onError = null)
			{
				try {
					var i = new Intent (Intent.ActionView);
					i.SetData (Android.Net.Uri.Parse (url));
					ctx.StartActivity (i);
				} catch (Exception e) {
					onError?.Invoke (ctx, e);
				}
			}

			public static void StartEmailClientActivity (Context ctx, string email, int chooseEmailClientStringId, IntentStartFailedDelegate onError = null)
			{
				try {
					var intent = new Intent (Intent.ActionSend);
					intent.PutExtra (Intent.ExtraEmail, new []{ email });
					intent.SetType ("message/rfc822");
					ctx.StartActivity (Intent.CreateChooser (intent, ctx.GetString (chooseEmailClientStringId)));
				} catch (Exception e) {
					onError?.Invoke (ctx, e);
				}
			}

			/// <summary>
			/// Starts image chooser from gallery. To catch the result of this intent (the image) you have to override the OnActivityResult of the fragment that started the intent. this is an example:
			/// 
			/// const int _requestCode = 123;
			/// public override async void OnActivityResult (int requestCode, int resultCode, Intent data)
			///	{
			///		base.OnActivityResult (requestCode, resultCode, data);
			/// 	if (requestCode == _requestCode AND resultCode == (int)Result.Ok) {
			///			_imageFilePath = ApplicationUtils.GetImagePathOnActivityResult (data);
			///			var bitmap = await BitmapUtils.ScaleToStreamAsync (_imageFilePath);
			/// 	}
			///	}
			/// </summary>
			/// <param name="ctx">Context.</param>
			/// <param name="requestCode">Request code.</param>
			/// <param name = "onError"></param>
			public static void StartImageFromGalleryChooser (Android.Support.V4.App.Fragment ctx, int requestCode, IntentStartFailedDelegate onError = null)
			{
				try {
					var intent = new Intent (Intent.ActionPick);
					intent.SetType ("image/*");
					ctx.StartActivityForResult (intent, requestCode);
				} catch (Exception e) {
					onError?.Invoke (ctx.Activity, e);
				}
			}
				
			public static void StartGoogleMapActivity (Context ctx, string address, IntentStartFailedDelegate onError = null)
			{
				try {
					ctx.StartActivity (new Intent (Intent.ActionView, Android.Net.Uri.Parse ("geo:?q=" + address)));
				} catch (Exception e) {
					onError?.Invoke (ctx, e);
				}
			}

			public static void StartDialerActivity (Context ctx, string number, IntentStartFailedDelegate onError = null)
			{
				try {
					ctx.StartActivity (new Intent (Intent.ActionDial, Android.Net.Uri.Parse ("tel:" + number)));
				} catch (Exception e) {
					onError?.Invoke (ctx, e);
				}
			}

			public static void StartSkypeCallActivity (Context ctx, string contact, IntentStartFailedDelegate onError = null)
			{
				try {
					var intent = new Intent (Intent.ActionView);
					intent.SetData (Android.Net.Uri.Parse ("skype:" + contact + "?call"));
					intent.SetComponent (new ComponentName ("com.skype.raider", "com.skype.raider.Main"));
					intent.SetFlags (ActivityFlags.NewTask);
					ctx.StartActivity (intent);
				} catch (Exception e) {
					onError?.Invoke (ctx, e);
				}
			}

			public static void StartFacebookPageOnBrowser (Context ctx, string facebookProfileId, IntentStartFailedDelegate onError = null)
			{
				try {
					StartBrowserActivity (ctx, "https://www.facebook.com/" + facebookProfileId, onError);
				} catch (Exception e) {
					onError?.Invoke (ctx, e);
				}
			}

			public static void StartEnableLocalizationActivity(Context ctx)
			{
				ctx.StartActivity (new Intent (Settings.ActionLocationSourceSettings));
			}

			public static void StartMarketPage (Context ctx, string packageNameNullIfCurrent = null)
			{
				var name = packageNameNullIfCurrent ?? ApplicationUtils.PackageInfo.PackageName;
				try {
					ctx.StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=" + name)));
				} 
				catch 
				{
					ctx.StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=" + name)));
				}
			}
		
			public static void StartGalleryIntent (Activity activity, int requestCode, int choosePictureExplorerStringId, IntentStartFailedDelegate onError = null)
			{
				try {
					var intent = new Intent ();
					intent.SetType ("image/*");
					intent.SetAction (Intent.ActionGetContent);
					activity.StartActivityForResult (Intent.CreateChooser (intent, activity.GetString (choosePictureExplorerStringId)), requestCode);
				}
				catch (Exception e) {
					onError?.Invoke (activity, e);
				}
			}

			public static string GetImagePathOnActivityResult (Context ctx, Intent data)
			{
				var selectedImage = data.Data;
				String[] filePathColumn = { MediaStore.MediaColumns.Data };
				var cursor = ctx.ContentResolver.Query (selectedImage, filePathColumn, null, null, null);
				string imageFilePath = null;
				if (cursor.MoveToFirst ()) {
					var columnIndex = cursor.GetColumnIndex (filePathColumn [0]);
					imageFilePath = cursor.GetString (columnIndex);
				}
				cursor.Close ();
				return imageFilePath;
			}

			public static Bitmap GetBitmapAfterCropping (Intent data)
			{
				var extras = data.Extras;  
				return extras != null ? (Bitmap)extras.GetParcelable ("data") : null;
			}

			public static string HandleGalleryActivityResult (int expectedResultCode, Context ctx, int requestCode, Result resultCode, Intent data)
			{
				if (resultCode == Result.Ok && requestCode == expectedResultCode)
					return FileChooserUtils.OnActivityResult (ctx, expectedResultCode, requestCode, (int)resultCode, data);

				return null;
			}

			/// <summary>
			/// Lanchs the camera intent. Throws exception (e.g. cant create the file)
			/// Example of usage:
			/// 
			/// string _filePath;
			/// void LanchIntent ()
			/// {
			/// 	_filePath = ApplicationUtils.Intents.LanchCameraIntent (this, 12, "/sdcard/", "myphoto.jpg");
			/// }
			/// 
			/// override OnActivityResult (...)
			/// {
			/// 	if (resultCode == Result.Ok && requestCode == 12 && !string.IsNullOrEmpty (_filePath))
			/// 		DoWhatIWantWithThePath (_filePath);
			/// }
			/// 
			/// </summary>
			/// <returns>The complete file path of the created file (the picture)</returns>
			/// <param name="activity">Activity.</param>
			/// <param name="requestCode">Request code.</param>
			/// <param name="outputDirectoryPath">Output directory path.</param>
			/// <param name="fileName">File name.</param>
			public static string StartCameraIntent (Activity activity, int requestCode, string outputDirectoryPath, string fileName, IntentStartFailedDelegate onError = null)
			{
				try {
					var newdir = new Java.IO.File (outputDirectoryPath);
					newdir.Mkdirs();

					var completeFilePath = System.IO.Path.Combine (outputDirectoryPath, fileName);
					var newfile = new Java.IO.File (completeFilePath);

					newfile.CreateNewFile();

					var outputFileUri = Android.Net.Uri.FromFile (newfile);

					var cameraIntent = new Intent (MediaStore.ActionImageCapture);
					cameraIntent.PutExtra (MediaStore.ExtraOutput, outputFileUri);

					activity.StartActivityForResult(cameraIntent, requestCode);

					return completeFilePath;
				}
				catch (Exception e) {
					onError?.Invoke (activity, e);
					return null;
				}
			}
		}
	}
}

