﻿using System;
using Android.App;
using Android.Gms.Gcm;
using Android.Gms.Common;
using Android.Content;
using MasDev.Exceptions;

namespace MasDev.Droid.Utils
{
	public static class GCMUtils
	{
		const string PROPERTY_REG_ID = "registration_id";
		const string PROPERTY_APP_VERSION = "appVersion";
		const string SHARED_PREFERENCES_NAME = "pqowwo";

		public static async System.Threading.Tasks.Task PerformRegistration (Activity ctx, int playServicesResolutionRequest, string senderId, Func<string, System.Threading.Tasks.Task> onRegistrationIdRetrived, Action onDeviceNotSupported)
		{
			string regid;
			if (CheckPlayServices (ctx, onDeviceNotSupported, playServicesResolutionRequest)) 
			{
				var gcm = GoogleCloudMessaging.GetInstance (ctx);
				regid = GetRegistrationIdLocally (ctx);

				if (string.IsNullOrEmpty (regid)) {
					await System.Threading.Tasks.Task.Run (() => {
						regid = gcm.Register (senderId);
						GetGCMPreferences (ctx).Edit ().PutString (PROPERTY_REG_ID, regid).Commit ();
					});
				}
				await onRegistrationIdRetrived.Invoke (regid);
			} 
		}

		static bool CheckPlayServices (Activity activity, Action onDeviceNotSupported, int playServicesResolutionRequest)
		{
			int resultCode = GooglePlayServicesUtil.IsGooglePlayServicesAvailable (activity);
			if (resultCode != ConnectionResult.Success) {
				if (GooglePlayServicesUtil.IsUserRecoverableError (resultCode)) {
					GooglePlayServicesUtil.GetErrorDialog (resultCode, activity, playServicesResolutionRequest).Show ();
				}
				onDeviceNotSupported.Invoke ();
				return false;
			}
			return true;
		}

		static string GetRegistrationIdLocally (Context context)
		{
			var prefs = GetGCMPreferences (context);
			var registrationId = prefs.GetString (PROPERTY_REG_ID, string.Empty);
			if (string.IsNullOrEmpty (registrationId))
				return string.Empty;

			int registeredVersion = prefs.GetInt (PROPERTY_APP_VERSION, int.MinValue);
			int currentVersion = GetAppVersion (context);
			return registeredVersion != currentVersion ? string.Empty : registrationId;
		}

		static ISharedPreferences GetGCMPreferences (Context context)
		{
			return context.GetSharedPreferences (SHARED_PREFERENCES_NAME, FileCreationMode.Private);
		}

		static int GetAppVersion (Context context)
		{
			try {
				var packageInfo = context.PackageManager.GetPackageInfo (context.PackageName, 0);
				return packageInfo.VersionCode;
			} 
			catch (Exception e) {
				throw new ShouldNeverHappenException ("Could not get package name: " + e);
			}
		}
	}
}
