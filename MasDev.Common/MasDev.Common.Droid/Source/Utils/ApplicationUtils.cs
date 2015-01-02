using System;
using Android.Content;
using Android.Content.Res;
using Android.App;
using Android.Views.InputMethods;
using System.Threading.Tasks;
using Android.Util;
using Android.Provider;
using Android.Graphics;
using Android.Views;

namespace MasDev.Droid.Utils
{
	public delegate void IntentStartFailedDelegate(Context ctx, Exception e);

	public static class ApplicationUtils
	{
		static Context _context;

		public static Context Context {
			get {
				if (_context == null)
					throw new NullReferenceException ("You must initialize the context first");
				return _context;
			}

			set {
				_context = value;
			}
		}

		public static Point ScreenSize (Activity activity)
		{
			var display = activity.WindowManager.DefaultDisplay;
			var size = new Point ();
			display.GetSize (size);
			return size;
		}

		static int _deviceIsTablet = -1;

		public static bool DeviceIsTablet ()
		{
			if (_deviceIsTablet == -1) {
				var x = Context.Resources.Configuration.ScreenLayout & ScreenLayout.SizeMask;
				_deviceIsTablet = ((x == ScreenLayout.SizeLarge) || (x == ScreenLayout.SizeXlarge)) ? 1 : 0;
			}
			return _deviceIsTablet == 1;
		}

		public static string GetApplicationFolderPath ()
		{
			return Environment.GetFolderPath (Environment.SpecialFolder.Personal);
		}

		public static void HideKeyboard (Activity context)
		{
			try {
				Task.Run (() => {
					((InputMethodManager)context.GetSystemService (Context.InputMethodService)).HideSoftInputFromWindow (context.Window.DecorView.WindowToken, HideSoftInputFlags.NotAlways);
				});
			} catch {}
		}

		public static void ShowKeyboard (Activity context)
		{
			try {
				Task.Run (() => {
					((InputMethodManager)context.GetSystemService (Context.InputMethodService)).ToggleSoftInput (ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
				});
			} catch {}
		}

		public static void CopyStringToClipBoard (string value)
		{
			var clipboard = (ClipboardManager)Context.GetSystemService (Context.ClipboardService);
			var clip = ClipData.NewPlainText (string.Empty, value);
			clipboard.PrimaryClip = clip;
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

		public static int ConvertDpToPixel (Context ctx, float dp)
		{
			return (int)TypedValue.ApplyDimension (ComplexUnitType.Dip, dp, ctx.Resources.DisplayMetrics);
		}

		public static decimal ScreenSizeInch (Context ctx) 
		{
			var dm = ctx.Resources.DisplayMetrics;

			double density = dm.Density * 160;
			double x = Math.Pow(dm.WidthPixels / density, 2);
			double y = Math.Pow(dm.HeightPixels / density, 2);
			return (decimal) Math.Sqrt(x + y);
		}
			
		public static class Intents
		{
			public static readonly IntentStartFailedDelegate VoidDelegate = (_, __) => {}; 

			public static void StartSettingsActivity (Activity ctx)
			{
				ctx.StartActivityForResult (new Intent (Settings.ActionSettings), 0);
			}

			public static void StartBrowserActivity (Context ctx, string site, IntentStartFailedDelegate onError)
			{
				try {
					var i = new Intent (Intent.ActionView);
					i.SetData (Android.Net.Uri.Parse (site));
					ctx.StartActivity (i);
				} catch (Exception e) {
					onError.Invoke (ctx, e);
				}
			}

			public static void StartEmailClientActivity (Context ctx, string email, int chooseEmailClientStringId, IntentStartFailedDelegate onError)
			{
				try {
					var intent = new Intent (Intent.ActionSend);
					intent.PutExtra (Intent.ExtraEmail, new []{ email });
					intent.SetType ("message/rfc822");
					ctx.StartActivity (Intent.CreateChooser (intent, ctx.GetString (chooseEmailClientStringId)));
				} catch (Exception e) {
					onError.Invoke (ctx, e);
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
			public static void StartImageFromGalleryChooser (Android.Support.V4.App.Fragment ctx, int requestCode, IntentStartFailedDelegate onError)
			{
				try {
					var intent = new Intent (Intent.ActionPick);
					intent.SetType ("image/*");
					ctx.StartActivityForResult (intent, requestCode);
				} catch (Exception e) {
					onError.Invoke (ctx.Activity, e);
				}
			}
				
			public static void StartGoogleMapActivity (Context ctx, string address, IntentStartFailedDelegate onError)
			{
				try {
					ctx.StartActivity (new Intent (Intent.ActionView, Android.Net.Uri.Parse ("geo:?q=" + address)));
				} catch (Exception e) {
					onError.Invoke (ctx, e);
				}
			}

			public static void StartDialerActivity (Context ctx, string number, IntentStartFailedDelegate onError)
			{
				try {
					ctx.StartActivity (new Intent (Intent.ActionDial, Android.Net.Uri.Parse ("tel:" + number)));
				} catch (Exception e) {
					onError.Invoke (ctx, e);
				}
			}

			public static void StartSkypeCallActivity (Context ctx, string contact, IntentStartFailedDelegate onError)
			{
				try {
					var intent = new Intent (Intent.ActionView);
					intent.SetData (Android.Net.Uri.Parse ("skype:" + contact + "?call"));
					intent.SetComponent (new ComponentName ("com.skype.raider", "com.skype.raider.Main"));
					intent.SetFlags (ActivityFlags.NewTask);
					ctx.StartActivity (intent);
				} catch (Exception e) {
					onError.Invoke (ctx, e);
				}
			}

			public static void StartFacebookPageOnBrowser (Context ctx, string facebookProfileId, IntentStartFailedDelegate onError)
			{
				try {
					StartBrowserActivity (ctx, "https://www.facebook.com/" + facebookProfileId, onError);
				} catch (Exception e) {
					onError.Invoke (ctx, e);
				}
			}

			public static void StartEnableLocalizationActivity(Context ctx)
			{
				ctx.StartActivity (new Intent (Settings.ActionLocationSourceSettings));
			}
		}
	}
}

