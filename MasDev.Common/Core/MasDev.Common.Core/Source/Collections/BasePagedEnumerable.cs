using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MasDev.Utils;

namespace MasDev.Collections
{
	public static class BasePagedEnumerable
	{
		public static IPagedEnumerable<T> Create<T> (int pageSize, Func<int, int, Task<IEnumerable<T>>> retrivePageDelegate)
		{
			return new DelegateBasePagedEnumerable<T> (pageSize, retrivePageDelegate);
		}
	}

	class DelegateBasePagedEnumerable<T> : BasePagedEnumerable<T>
	{
		readonly Func<int, int, Task<IEnumerable<T>>> _retrivePageDelegate;

		public DelegateBasePagedEnumerable (int pageSize, Func<int, int, Task<IEnumerable<T>>> retrivePageDelegate) : base (pageSize)
		{
			_retrivePageDelegate = retrivePageDelegate;
		}

		public override async Task<IEnumerable<T>> RetrivePageAsync (int currentPage)
		{
			return await _retrivePageDelegate.Invoke (currentPage, PageSize);
		}
	}

	public abstract class BasePagedEnumerable<T> : IPagedEnumerable<T>
	{
		bool _finished;

		public int PageSize { get; set; }

		public int CurrentPage { get; set; }

		public bool HasMorePages { get { return !_finished; } }

		public List<T> Items { get; set; }

		protected BasePagedEnumerable (int pageSize)
		{
			CurrentPage = 0;
			PageSize = pageSize;

			Items = new List<T> ();
		}

		public virtual void Reset ()
		{
			CurrentPage = 0;
			_finished = false;

			Items.Clear ();
		}

		public abstract Task<IEnumerable<T>> RetrivePageAsync (int currentPage);

		public async Task<IEnumerable<T>> GetNextPageAsync ()
		{
			if (_finished)
				throw new Exception ("No more pages");

			try {
				var page = await RetrivePageAsync (CurrentPage++);

				var hasPageItems = !CollectionUtils.IsNullOrEmpty(page);

				_finished |= page == null || !hasPageItems;

				if(page != null && hasPageItems)
					AddPage(page);

				return page;
			} catch (Exception) {
				CurrentPage--;
				throw;
			}
		}

		protected virtual void AddPage (IEnumerable<T> page)
		{
			Items.AddRange (page);
		}

		public virtual bool RemoveItem (T item)
		{
			return Items.Remove (item);
		}

		public virtual void AddItem (T item, bool addOnTop)
		{
			if (addOnTop)
				Items.Insert (0, item);
			else 
				Items.Add (item);
		}
	}
}