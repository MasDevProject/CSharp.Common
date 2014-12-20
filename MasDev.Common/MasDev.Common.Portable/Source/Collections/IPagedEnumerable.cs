using System.Collections.Generic;
using System.Threading.Tasks;

namespace MasDev.Common.Collections
{
	public interface IPagedEnumerable<T>
	{
		int CurrentPage { get; set; }

		int PageSize { get; set; }

		bool HasMorePages { get; }

		Task<IEnumerable<T>> GetNextPageAsync ();

		void Reset ();
	}
}

