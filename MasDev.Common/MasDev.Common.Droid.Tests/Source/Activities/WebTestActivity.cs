using Android.App;
using Android.Widget;
using Android.OS;
using MasDev.Common.Share.Tests;

namespace MasDev.Common.Droid.Tests
{
	[Activity]
	public class WebTestActivity : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.WebTestActivityLayout);

			var button = FindViewById<Button> (Resource.Id.btnFire);
			var lblResponse = FindViewById<TextView> (Resource.Id.lblResponse);
		
			button.Click += async (sender, e) => lblResponse.Text = await GoogleAPIs.GetGoogleHomePage();
		}
	}
}


