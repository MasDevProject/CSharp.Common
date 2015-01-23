using Android.Widget;
using System;
using Android.Views;
using Android.Content;
using MasDev.Collections;
using System.Linq;
using MasDev.Extensions;
using MasDev.Droid.ExtensionMethods;

namespace MasDev.Droid.Utils
{
	public abstract class DefaultPagedLoadingAdapter<T> : PagedAdapter<T>
	{
		Action _loadingUi;
		Action _loadedUi;
		Action _noResultUi;
		Action _errorUi;
		ViewGroup _footerLayout;

		protected abstract View ListViewError { get;}
		protected abstract ProgressBar ListViewLoading {get;}

		protected ListView MainListView;
		protected Context Context { get; private set; }

		protected DefaultPagedLoadingAdapter (Context ctx, IPagedEnumerable<T> paged, ListView listView, Action<object, AdapterView.ItemClickEventArgs> onItemClick, Action loadingUi, Action loadedUi, Action noResultUi, Action errorUi, Func<T, T, bool> comparer) : base(ctx, paged, comparer)
		{
			Context = ctx;

			_loadingUi = loadingUi;
			_loadedUi = loadedUi;
			_noResultUi = noResultUi;
			_errorUi = errorUi;
			MainListView = listView;

			OnLoading += HandleOnLoading;
			OnLoaded += HandleOnLoaded;
			OnError += HandleOnError;

			_footerLayout = new FrameLayout(ctx);
			_footerLayout.AddView (ListViewError);
			_footerLayout.AddView (ListViewLoading);

			MainListView.AddFooterView (_footerLayout);
			MainListView.ItemClick += onItemClick.Invoke;
			MainListView.Adapter = this;
		}

		void HandleOnLoading (bool firstLoad)
		{
			if (firstLoad || !Items.Any())
				_loadingUi.Invoke ();
			else {
				_footerLayout.SetVisible ();
				ListViewLoading.SetVisible ();
				ListViewError.SetGone ();
			}
		}

		void HandleOnLoaded (System.Collections.Generic.IEnumerable<T> obj)
		{
			if (Items.Any())
				_loadedUi.Invoke ();
			else 
				_noResultUi.Invoke ();
			_footerLayout.SetGone ();
		}

		void HandleOnError (Exception obj)
		{
			if (!Items.Any ())
				_errorUi.Invoke ();
			else {
				_footerLayout.SetVisible ();
				ListViewError.SetVisible ();
				ListViewLoading.SetGone ();
			}
		}

		protected void HandleRetryButtonClick (object sender, EventArgs e)
		{
			LoadNextPageAsync ();
		}

		protected override void Dispose (bool disposing)
		{
			_loadingUi = null;
			_loadedUi = null;
			_noResultUi = null;
			_errorUi = null;
			_footerLayout.DisposeIfNotNull ();
			_footerLayout = null;
			MainListView.DisposeIfNotNull();
			MainListView = null;
			Context = null;
			base.Dispose (disposing);
		}
	}
}

