using System;
using Foundation;

namespace MasDev.iOS.Extensions
{
	public static class DateExtensions
	{
		private static DateTime ReferenceDate 
		{
			get { return new DateTime (2001, 1, 1, 0, 0, 0, DateTimeKind.Utc); }
		}

		// NSDate extensions

		public static DateTime ToDateTime(this NSDate nsDateUTC)
		{
			return ReferenceDate.AddSeconds(nsDateUTC.SecondsSinceReferenceDate).ToLocalTime();
		}

		// DateTime extensions

		public static NSDate ToNSDate(this DateTime date)
		{
			date = date.ToUniversalTime ();

			return NSDate.FromTimeIntervalSinceReferenceDate((date - ReferenceDate).TotalSeconds);
		}

		public static DateTime SumDate(this DateTime date, DateTime source)
		{
			return date.AddSeconds (source.Second).AddMinutes (source.Minute).AddHours (source.Hour);
		}
	}
}