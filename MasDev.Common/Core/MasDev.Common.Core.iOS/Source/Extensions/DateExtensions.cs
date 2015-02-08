using System;
using Foundation;

namespace MasDev.iOS.Extensions
{
	public static class DateExtensions
	{
		private static DateTime BaseDate = new DateTime(2001,1,1,0,0,0);

		public static NSDate ToNSDate(this DateTime dateTime)
		{
			return NSDate.FromTimeIntervalSinceReferenceDate((dateTime-(BaseDate)).TotalSeconds);
		}

		public static DateTime ToDateTime(this NSDate nsDate)
		{
			return (BaseDate).AddSeconds(nsDate.SecondsSinceReferenceDate);
		}
	}
}