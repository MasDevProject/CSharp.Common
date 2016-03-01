using System.Collections.Generic;
using System.Threading.Tasks;

namespace MasDev.Collections
{
	public interface IPagedEnumerable<T>
	{
		int CurrentPage { get; set; }

		int PageSize { get; }

		bool HasMorePages { get; }

		List<T> Items { get; }

		Task<IEnumerable<T>> GetNextPageAsync ();

		void Reset ();
	}
}