using System;
using Android.App;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.Gms.Plus;
using Android.OS;
using Android.Content;
using System.Threading.Tasks;
using Android.Gms.Auth;
using Android.Accounts;

namespace MasDev.Droid.App
{
	public class GoogleLoginActivity : Activity, IGoogleApiClientConnectionCallbacks, IGoogleApiClientOnConnectionFailedListener
	{
		bool _intentInProgress;
		bool _signInClicked;
		IGoogleApiClient _apiClient;
		ConnectionResult _connectionResult;

		const int RC_SIGN_IN = 0;
		const int PROFILE_PIC_SIZE = 400;

		public const string BundleKey = "0";
		public const string FirstNameKey = "a";
		public const string LastNameKey = "a2";
		public const string AvatarUrlKey = "b";
		public const string ProfileUrlKey = "c";
		public const string EmailKey = "d";
		public const string IdKey = "e";
		public const string BirthDayKey = "f";
		public const string AboutMeKey = "g";
		public const string CoverImageUrlKey = "h";
		public const string GenderKey = "i";
		public const string IsPlusUser = "l";
		public const string AccessTokenKey = "m";

		protected void SignInToGplus ()
		{
			if (!_apiClient.IsConnecting)
				_apiClient.Connect ();
		}

		protected void SignOutFromGplus ()
		{
			if (_apiClient.IsConnected) {
				_apiClient.ClearDefaultAccountAndReconnect ();
				_apiClient.Disconnect ();
			}
			SetResult (Result.Canceled);
			Finish ();
		}

		protected void RevokeGplusAccess ()
		{
			_apiClient.ClearDefaultAccountAndReconnect ();
			PlusClass.AccountApi.RevokeAccessAndDisconnect (_apiClient);
			SetResult (Result.Canceled);
			Finish ();
		}

		#region Private stuff

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			_apiClient = new GoogleApiClientBuilder (this)
				.AddConnectionCallbacks (this)
				.AddOnConnectionFailedListener (this)
				.AddApi (PlusClass.API)
				.AddScope (PlusClass.ScopePlusLogin)
				.Build ();
		}

		protected override void OnStop ()
		{
			base.OnStop ();
			if (_apiClient.IsConnected)
				_apiClient.Disconnect ();
		}

		public async void OnConnected (Bundle connectionHint)
		{
			_signInClicked = false;
			await GetProfileInformation ();
		}

		public void OnConnectionSuspended (int cause)
		{
			_apiClient.Connect ();
		}

		int dialogShowedCount;
		public void OnConnectionFailed (ConnectionResult result)
		{
			if (!result.HasResolution) {
				GoogleApiAvailability.Instance.GetErrorDialog (this, result.ErrorCode, 0).Show ();
				return;
			}

			if (dialogShowedCount < 2) {
				result.StartResolutionForResult (this, RC_SIGN_IN);
				dialogShowedCount++;
			} else {
				SetResult (Result.Canceled);
				Finish ();
			}

			if (!_intentInProgress) {
				_connectionResult = result;
				if (_signInClicked)
					ResolveSignInError ();
			}
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			if (requestCode == RC_SIGN_IN) {
				_signInClicked &= resultCode == Result.Ok;

				_intentInProgress = false;

				if (!_apiClient.IsConnecting)
					_apiClient.Connect ();
			}
		}

		void ResolveSignInError ()
		{
			if (_connectionResult.HasResolution) {
				try {
					_intentInProgress = true;
					_connectionResult.StartResolutionForResult (this, RC_SIGN_IN);
				} catch (Exception) {
					_intentInProgress = false;
					_apiClient.Connect ();
				}
			}
		}

		async Task GetProfileInformation ()
		{
			var currentPerson = PlusClass.PeopleApi.GetCurrentPerson (_apiClient);
			if (currentPerson != null) {
				var firstName = currentPerson.HasName ? (currentPerson.Name.HasGivenName ? currentPerson.Name.GivenName : null) : null;
				var lastName = currentPerson.HasName ? (currentPerson.Name.HasFamilyName ? currentPerson.Name.FamilyName : null) : null;

				var personPhotoUrl = currentPerson.HasImage ? currentPerson.Image.Url : null;
				var personGoogleProfileUrl = currentPerson.HasUrl ? currentPerson.Url : null;
				var email = PlusClass.AccountApi.GetAccountName (_apiClient);
				var id = currentPerson.HasId ? currentPerson.Id : null;
				var birthDay = currentPerson.HasBirthday ? currentPerson.Birthday : null;
				var aboutMe = currentPerson.HasAboutMe ? currentPerson.AboutMe : null;
				var currentPhotoUrl = currentPerson.HasCover ? currentPerson.Cover.CoverPhoto.Url : null;
				int gender = currentPerson.HasGender ? currentPerson.Gender : -1;
				bool isPlusUser = currentPerson.IsPlusUser;

				if (personPhotoUrl != null && personPhotoUrl.Contains ("sz="))
					personPhotoUrl = personPhotoUrl.Substring (0, personPhotoUrl.Length - 2) + "256";

				var token = await Task.Run (() => {
					var account = new Account(email, GoogleAuthUtil.GoogleAccountType);
					return GoogleAuthUtil.GetToken (ApplicationContext, account, "oauth2:" + Scopes.PlusLogin);
				});

				var intent = new Intent ();
				var bundle = new Bundle ();

				bundle.PutString (FirstNameKey, firstName);
				bundle.PutString (LastNameKey, lastName);
				bundle.PutString (AvatarUrlKey, personPhotoUrl);
				bundle.PutString (ProfileUrlKey, personGoogleProfileUrl);
				bundle.PutString (EmailKey, email);
				bundle.PutString (IdKey, id);
				bundle.PutString (BirthDayKey, birthDay);
				bundle.PutString (AboutMeKey, aboutMe);
				bundle.PutString (CoverImageUrlKey, currentPhotoUrl);
				bundle.PutInt (GenderKey, gender);
				bundle.PutBoolean (IsPlusUser, isPlusUser);
				bundle.PutString (AccessTokenKey, token);

				intent.PutExtra (BundleKey, bundle);

				SetResult (Result.Ok, intent);
				Finish ();
			}
		}

		#endregion
	}
}

