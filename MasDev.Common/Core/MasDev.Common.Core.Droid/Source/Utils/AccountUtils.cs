using System.Collections.Generic;
using Android.Content;
using Android.Accounts;
using Android.Telephony;

namespace MasDev.Droid.Utils
{
	public static class AccountUtils
	{
		public static ICollection<string> GetEmails (Context ctx)
		{
			var emails = new List<string> ();
			var pattern = Android.Util.Patterns.EmailAddress;
			foreach (var account in AccountManager.Get (ctx).GetAccounts ()) {
				if (pattern.Matcher (account.Name).Matches ())
					emails.Add (account.Name);
			}
			return emails;
		}

		/// <summary>
		/// Require permissions: <uses-permission android:name="android.permission.READ_PHONE_STATE"/> 
		/// This method does not work with all devices or telephony provider. In that cases it returns a blank string
		/// </summary>
		public static string GetPhoneNumber (Context ctx)
		{
			var tMgr = (TelephonyManager)ctx.GetSystemService(Context.TelephonyService);
			return tMgr.Line1Number;
		}
	}
}

