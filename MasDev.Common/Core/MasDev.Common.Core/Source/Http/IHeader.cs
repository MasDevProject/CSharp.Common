using System.Collections.Generic;

namespace MasDev.Common.Http
{
	public interface IHeader
	{
		string Name { get; }

		IEnumerable<string> Values { get; }
	}
}

