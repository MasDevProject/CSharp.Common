using System;
using System.Collections.Generic;

namespace MasDev.Utils
{
	public interface ITimeZoneConverter
	{
		TimeZoneInfo FromTzdbId (string tzdbId);

		IEnumerable<TimeZoneInfo> All { get;}
	}
}

