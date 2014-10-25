using System;
using MonoTouch.UIKit;
using System.Text;
using MasDev.Common.Share.Tests;

namespace MasDev.Common.iOS.Tests
{
	partial class DatabaseTestController : UIViewController
	{
		public DatabaseTestController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			btnInsertEntry.TouchDown += async (sender, e) => {
				var tc = new TestClass () { Text = edtEntry.Text };
				await RepoManager.PersonRepository.CreateAsync(tc);
			};

			btnGetAll.TouchDown += async (sender, e) => {
				var list = await RepoManager.PersonRepository.DataSet.ToListAsync();
				var sb = new StringBuilder ();
				list.ForEach ((i) => sb.Append (string.Format ("{0} {1}\n", i.Id, i.Text)));
				sb.Append ("============================== where id < 17 && ud > 21\n");
				list = await RepoManager.PersonRepository.DataSet.Where(item => item.Id > 17 && item.Id < 21).ToListAsync();
				list.ForEach ((i) => sb.Append (string.Format ("{0} {1}\n", i.Id, i.Text)));

				lblResults.Text = sb.ToString();
			};
		}
	}
}
