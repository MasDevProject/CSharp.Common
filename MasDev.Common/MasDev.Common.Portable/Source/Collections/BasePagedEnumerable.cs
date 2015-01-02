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

		public void Reset ()
		{
			CurrentPage = 0;
			_finished = false;
		}

		protected BasePagedEnumerable (int pageSize)
		{
			CurrentPage = 0;
			PageSize = pageSize;
		}

		public abstract Task<IEnumerable<T>> RetrivePageAsync (int currentPage);

		public async Task<IEnumerable<T>> GetNextPageAsync ()
		{
			if (_finished)
				throw new Exception ("No more pages");

			try {
				var page = await RetrivePageAsync (CurrentPage++);
				_finished |= CollectionUtils.IsNullOrEmpty<T> (page);
				return page;
			} catch (Exception) {
				CurrentPage--;
				throw;
			}
		}

		public bool HasMorePages { get { return !_finished; } }
	}
}

