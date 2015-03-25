using System;
using Android.App;
using Android.Gms.Gcm;
using Android.Content;
using System.Threading.Tasks;


namespace MasDev.Droid.Utils
{
	public static class GCMUtils
	{
		const string PROPERTY_REG_ID = "registration_id";
		const string PROPERTY_APP_VERSION = "appVersion";
		const string SHARED_PREFERENCES_NAME = "pqowwo";

		public static async Task PerformRegistration (Activity ctx, string senderId, Func<string, Task> onRegistrationIdRetrived)
		{
			var regId = GetRegistrationId(ctx);
			if (string.IsNullOrEmpty (regId))
				await RegisterInBackground(ctx, senderId, onRegistrationIdRetrived);
		}

		static string GetRegistrationId (Context ctx)
		{
			var prefs = GetGCMPreferences(ctx);
			var registrationId = prefs.GetString(PROPERTY_REG_ID, string.Empty);
			if (string.IsNullOrEmpty (registrationId))
				return string.Empty;
			
			// Check if app was updated; if so, it must clear the registration ID
			// since the existing registration ID is not guaranteed to work with
			// the new app version.
			int registeredVersion = prefs.GetInt(PROPERTY_APP_VERSION, int.MinValue);
			int currentVersion = ApplicationUtils.PackageInfo.VersionCode;
			return registeredVersion != currentVersion ? string.Empty : registrationId;
		}

		static async Task RegisterInBackground (Context ctx, string senderId, Func<string, Task> onRegistrationIdRetrived)
		{
			var regid = await Task.Run (() =>  { return GoogleCloudMessaging.GetInstance(ctx).Register(senderId); });
			await onRegistrationIdRetrived(regid);
			storeRegistrationId(ctx, regid);
		}

		static ISharedPreferences GetGCMPreferences (Context ctx)
		{
			return ctx.GetSharedPreferences (SHARED_PREFERENCES_NAME, FileCreationMode.Private);
		}

		static void storeRegistrationId (Context ctx, string regid)
		{
			var prefs = GetGCMPreferences(ctx);
			var appVersion = ApplicationUtils.PackageInfo.VersionCode;
			var editor = prefs.Edit();
			editor.PutString(PROPERTY_REG_ID, regid);
			editor.PutInt(PROPERTY_APP_VERSION, appVersion);
			editor.Commit();	
		}
	}
}

