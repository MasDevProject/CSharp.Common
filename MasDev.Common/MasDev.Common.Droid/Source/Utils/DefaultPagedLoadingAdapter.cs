using MasDev.Common.Droid.Utils;
using Android.Widget;
using System;
using Android.Views;
using Android.Content;
using MasDev.Common.Collections;
using System.Linq;
using MasDev.Common.Extensions;

namespace MasDev.Common.Droid.Utils
{
	public abstract class DefaultPagedLoadingAdapter<T> : PagedAdapter<T>
	{
		Action _loadingUi;
		Action _loadedUi;
		Action _noResultUi;
		Action _errorUi;

		protected abstract ProgressBar ListViewLoading {get;}
		protected abstract View ListViewError { get;}

		protected ListView ListView;
		protected Context Context { get; private set; }

		protected DefaultPagedLoadingAdapter (Context ctx, IPagedEnumerable<T> paged, ListView listView, Action<object, AdapterView.ItemClickEventArgs> onItemClick, Action loadingUi, Action loadedUi, Action noResultUi, Action errorUi) : base(ctx, paged)
		{
			Context = listView.Context;

			_loadingUi = loadingUi;
			_loadedUi = loadedUi;
			_noResultUi = noResultUi;
			_errorUi = errorUi;
			ListView = listView;

			OnLoading += HandleOnLoading;
			OnLoaded += HandleOnLoaded;
			OnError += HandleOnError;

			ListView.ItemClick += onItemClick.Invoke;
			ListView.Adapter = this;
		}

		void HandleOnLoading (bool firstLoad)
		{
			if (firstLoad || !Items.Any())
				_loadingUi.Invoke ();
			else {
				ListView.AddFooterView (ListViewLoading);
				ListView.RemoveFooterView (ListViewError);
			}
		}

		void HandleOnLoaded (System.Collections.Generic.IEnumerable<T> obj)
		{
			if (Items.Any())
				_loadedUi.Invoke ();
			else 
				_noResultUi.Invoke ();
			ListView.RemoveFooterView (ListViewLoading);
			ListView.RemoveFooterView (ListViewError);
		}

		void HandleOnError (Exception obj)
		{
			if (!Items.Any())
				_errorUi.Invoke ();
			else
				ListView.AddFooterView (ListViewError);

			ListView.RemoveFooterView (ListViewLoading);
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

			ListView.DisposeIfNotNull();
			ListView = null;
			Context = null;
			base.Dispose (disposing);
		}
	}
}

