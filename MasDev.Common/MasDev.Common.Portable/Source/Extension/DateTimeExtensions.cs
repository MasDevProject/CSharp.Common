using System;

namespace MasDev.Extensions
{
	public static class DateTimeExtensions
	{
		public static bool IsSameDay(this DateTime t, DateTime dt)
		{
			return t.Year == dt.Year && t.Month == dt.Month && t.Day == dt.Day;
		}

		public static string TimeToString (this DateTime dt)
		{
			return dt.ToString ("HH:mm");
		}

		public static string DateToString (this DateTime dt)
		{
			return dt.ToString ("dd-MMMM-yyyy");
		}

		public static long GetMillisecondsSince1970(this DateTime dt) 
		{
			return (long) dt.ToUniversalTime ().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
		}
	}
}

