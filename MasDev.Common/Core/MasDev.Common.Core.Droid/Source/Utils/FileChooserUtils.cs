using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Database;
using MasDev.Utils;

namespace MasDev.Droid.Utils
{
	public static class FileChooserUtils
	{
		public static void ShowFileChooser (Activity activity, int requestCode, int fileChooserTitleId, int fileManagerNotFoundMessageId, int fileManagerNotFoundTitleid, int filemanagerNotFoundPositiveButtonid, int filemanagerNotFoundNegativeButtonid)
		{
			var intent = new Intent (Intent.ActionGetContent); 
			intent.SetType ("*/*"); 
			intent.AddCategory (Intent.CategoryOpenable);

			try {
				activity.StartActivityForResult (Intent.CreateChooser(intent, activity.GetString (fileChooserTitleId)), requestCode);
			} 
			catch {
				DialogUtils.ShowDialog (activity, fileManagerNotFoundMessageId, fileManagerNotFoundTitleid, true, filemanagerNotFoundPositiveButtonid, filemanagerNotFoundNegativeButtonid, () => {
					const string query = "file manager";
					try {
						activity.StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://search?q=" + query)));
					} catch {
						activity.StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://play.google.com/store/search?q=" + query)));
					}
				}, DelegateUtils.Void);
			}
		}

		public static string OnActivityResult (Context ctx, int expectedRequestCode, int requestCode, int resultCode, Intent data)
		{
			if (requestCode != expectedRequestCode || resultCode != (int) Result.Ok || data == null)
				return null;

			return GetPath (ctx, data.Data);
		}

		static string GetPath (Context context, Android.Net.Uri uri)
		{
			// check here to KITKAT or new version
			var isKitKat = Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat;

			// DocumentProvider
			if (isKitKat && DocumentsContract.IsDocumentUri (context, uri)) 
			{
				// ExternalStorageProvider
				if (IsExternalStorageDocument (uri)) 
				{
					var split = DocumentsContract.GetDocumentId (uri).Split (':');
					var type = split[0];

					if ("primary".Equals (type, StringComparison.CurrentCultureIgnoreCase)) 
						return Android.OS.Environment.ExternalStorageDirectory + "/" + split[1];
				}

				// DownloadsProvider
				else if (IsDownloadsDocument (uri)) 
				{
					var id = DocumentsContract.GetDocumentId (uri);
					var contentUri = ContentUris.WithAppendedId (Android.Net.Uri.Parse("content://downloads/public_downloads"), long.Parse (id));
					return GetDataColumn (context, contentUri, null, null);
				}

				// MediaProvider
				else if (IsMediaDocument (uri)) 
				{
					var split = DocumentsContract.GetDocumentId (uri).Split (':');
					var type = split[0];

					Android.Net.Uri contentUri = null;
					switch (type) {
					case "image":
						contentUri = MediaStore.Images.Media.ExternalContentUri;
						break;
					case "video":
						contentUri = MediaStore.Video.Media.ExternalContentUri;
						break;
					case "audio":
						contentUri = MediaStore.Audio.Media.ExternalContentUri;
						break;
					}    

					const string selection = "_id=?";
					var selectionArgs = new [] { split[1] };
					return GetDataColumn (context, contentUri, selection, selectionArgs);
				}
			}

			// MediaStore (and general)
			else if ("content".Equals (uri.Scheme, StringComparison.CurrentCultureIgnoreCase)) 
				return IsGooglePhotosUri (uri) ? uri.LastPathSegment : GetDataColumn (context, uri, null, null);

			// File
			else if ("file".Equals (uri.Scheme, StringComparison.CurrentCultureIgnoreCase))
				return uri.Path;

			return null;
		}

		static string GetDataColumn (Context context, Android.Net.Uri uri, String selection, String[] selectionArgs) 
		{
			ICursor cursor = null;
			const string column = "_data";
			string[] projection = { column };

			try {
				cursor = context.ContentResolver.Query (uri, projection, selection, selectionArgs, null);
				if (cursor != null && cursor.MoveToFirst ()) 
					return cursor.GetString (cursor.GetColumnIndexOrThrow (column));
			} 
			finally {
				if (cursor != null) 
					cursor.Close ();
			}
			return null;
		}    

		static bool IsExternalStorageDocument (Android.Net.Uri uri) 
		{
			return "com.android.externalstorage.documents" == uri.Authority;
		}

		static bool IsDownloadsDocument (Android.Net.Uri uri) 
		{
			return "com.android.providers.downloads.documents" == uri.Authority;
		}

		static bool IsMediaDocument (Android.Net.Uri uri) 
		{
			return "com.android.providers.media.documents" == uri.Authority;
		}

		static bool IsGooglePhotosUri (Android.Net.Uri uri) 
		{
			return "com.google.android.apps.photos.content" == uri.Authority;
		}
	}
}

