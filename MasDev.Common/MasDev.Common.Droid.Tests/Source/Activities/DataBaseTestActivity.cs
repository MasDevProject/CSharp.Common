using Android.App;
using Android.Widget;
using Android.OS;
using System.Text;
using MasDev.Common.Share.Tests;

namespace MasDev.Common.Droid.Tests
{
	[Activity]
	public class DataBaseTestActivity : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.DatabaseTestActivityLayout);

			var btnInsert = FindViewById<Button> (Resource.Id.btnInsert);
			var btnReadAll = FindViewById<Button> (Resource.Id.btnReadAll);
			var lblResults = FindViewById<TextView> (Resource.Id.lblResults);
			var txtInputText = FindViewById<EditText> (Resource.Id.txtInput);

			btnInsert.Click += async delegate
			{
				var tc = new TestClass { Text = txtInputText.Text };
				await RepoManager.PersonRepository.CreateAsync (tc);
			};

			btnReadAll.Click += async (sender, e) =>
			{
				var list = await RepoManager.PersonRepository.DataSet.Where (i => i.Id < 5).ToListAsync ();
				var sb = new StringBuilder ();
				list.ForEach (i => sb.Append (string.Format ("{0} {1}\n", i.Id, i.Text)));
				sb.Append ("==============================\n");
				(await RepoManager.PersonRepository.DataSet.ToListAsync ()).ForEach (i => sb.Append (string.Format ("{0} {1}\n", i.Id, i.Text)));
				lblResults.Text = sb.ToString ();
			};
		}
	}
}


