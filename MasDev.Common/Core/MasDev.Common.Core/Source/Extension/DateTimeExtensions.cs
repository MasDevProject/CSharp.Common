using System;
using System.Globalization;

namespace MasDev.Extensions
{
	public static class DateTimeExtensions
	{
		public static bool IsSameDay (this DateTime t, DateTime dt)
		{
			return t.Year == dt.Year && t.Month == dt.Month && t.Day == dt.Day;
		}
			
		/// <summary>
		/// It uses the current culture to format the time
		/// </summary>
		/// <returns>The short time string.</returns>
		/// <param name="dateTime">Date time.</param>
		public static string ToShortTimeString (this DateTime dateTime)
		{
			return dateTime.ToString("t", DateTimeFormatInfo.CurrentInfo);
		}
	}
}

