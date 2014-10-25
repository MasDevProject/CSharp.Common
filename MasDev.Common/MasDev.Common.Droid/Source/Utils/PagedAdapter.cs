using System;
using Android.Widget;
using Android.Content;
using MasDev.Common.Collections;
using System.Collections.Generic;
using Android.Views;
using MasDev.Common.Injection;

namespace MasDev.Common.Droid.Utils
{
	public abstract class PagedAdapter<T> : BaseAdapter, IFilterable
	{
		LayoutInflater _inflater;

		public Filter Filter { get; set; }
		public List<T> Items { get; set; }

		IPagedEnumerable<T> _paged;
		bool _firstLoad = true;

		public event Action<bool> OnLoading = delegate {};
		public event Action<IEnumerable<T>> OnLoaded = delegate {};
		public event Action<Exception> OnError = delegate {};

		readonly int _pageSize;

		protected PagedAdapter (Context ctx, IPagedEnumerable<T> paged)
		{
			_paged = paged;
			Items = new List<T> ();
			_inflater = (LayoutInflater)ctx.GetSystemService (Context.LayoutInflaterService);
			_pageSize = paged.PageSize;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = GetView (position, convertView, parent, _inflater);

			if (position % _pageSize == 0)
			if (position == Items.Count - _pageSize)
			if (position >= (_paged.CurrentPage-1) * _pageSize)
				LoadNextPageAsync ();

			return view;
		}

		public abstract View GetView (int position, View convertView, ViewGroup parent, LayoutInflater inflater);

		public T this [int index] { get { return Items [index]; } }

		public async void LoadNextPageAsync ()
		{
			if (!_paged.HasMorePages)
				return;

			try {
				OnLoading.Invoke (_firstLoad);
				var newItems = await _paged.GetNextPageAsync ();

				if (newItems != null)
					Items.AddRange (newItems);

				NotifyDataSetChanged ();
				OnLoaded.Invoke (newItems);
				_firstLoad = false;
			} 
			catch (Exception e) {
				OnError.Invoke (e);
				_firstLoad |= Items.Count <= 0;

				var logger = Injector.Resolve<ILogger> ();
				if (logger != null)
					logger.Log (e);
			}
		}

		public async void Reset ()
		{
			_paged.Reset ();
			_firstLoad = true;
		}

		public bool Any { get { return Items != null && Items.Count > 0; } }

		public override long GetItemId (int position)
		{
			return position;
		}

		public override int Count { get { return Items != null ? Items.Count : 0; } }

		public override Java.Lang.Object GetItem (int position) { return null; }

		protected override void Dispose (bool disposing)
		{
			if (_inflater != null) {
				_inflater.Dispose ();
				_inflater = null;
			}
			if (Filter != null) {
				Filter.Dispose ();
				Filter = null;
			}
			Items = null;
			_paged = null;
			OnLoading = null;
			OnLoaded = null;
			OnError = null;
			base.Dispose (disposing);
		}
	}
}

