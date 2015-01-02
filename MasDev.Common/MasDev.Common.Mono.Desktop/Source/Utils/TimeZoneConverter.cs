using MasDev.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MasDev.Utils
{
	public class TimeZoneConverter : ITimeZoneConverter
	{
		const string UTC = "UTC";
		static readonly string[] _utcZones = {"Etc/UTC", "Etc/UCT"};

		public TimeZoneInfo FromTzdbId (string tzdbId)
		{
			try {
				return TimeZoneInfo.FindSystemTimeZoneById (tzdbId) ?? Windows (tzdbId);
			}
			catch {
				return Windows (tzdbId);
			}
		}

		public IEnumerable<TimeZoneInfo> All { get { return TimeZoneInfo.GetSystemTimeZones (); }}

		static TimeZoneInfo Windows (string id)
		{
			var actualId = IanaToWindows (id);
			return TimeZoneInfo.FindSystemTimeZoneById (actualId);
		}

		static string IanaToWindows(string ianaZoneId)
		{
			if (_utcZones.Contains (ianaZoneId, StringComparer.OrdinalIgnoreCase))
				return UTC;

			var tzdbSource = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default;

			var links = tzdbSource.CanonicalIdMap
				.Where(x => x.Value.Equals(ianaZoneId, StringComparison.OrdinalIgnoreCase))
				.Select(x => x.Key);

			var mappings = tzdbSource.WindowsMapping.MapZones;
			var item = mappings.FirstOrDefault(x => x.TzdbIds.Any(links.Contains));
			return item == null ? null : item.WindowsId;
		}
	}
}

