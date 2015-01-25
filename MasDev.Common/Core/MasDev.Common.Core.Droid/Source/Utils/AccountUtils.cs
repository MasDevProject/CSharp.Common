using System.Collections.Generic;
using Android.Content;
using Android.Accounts;

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
	}
}

