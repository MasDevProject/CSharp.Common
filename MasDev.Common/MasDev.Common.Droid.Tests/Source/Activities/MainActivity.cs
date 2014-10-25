using Android.App;
using Android.Widget;
using Android.OS;

namespace MasDev.Common.Droid.Tests
{
	[Activity (Label = "MasDev.Common.Droid.Tests", MainLauncher = true)]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.Main);

			FindViewById<Button> (Resource.Id.btnDatabaseTest).Click += (sender, e) => StartActivity (typeof(DataBaseTestActivity));
			FindViewById<Button> (Resource.Id.btnWebTest).Click += (sender, e) => StartActivity (typeof(WebTestActivity));
		}
	}
}


