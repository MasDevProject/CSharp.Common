using System;

namespace MasDev.Utils
{

	public struct ClockTime
	{
		readonly uint _hours;
		readonly uint _minutes;

		public uint Hour { get { return _hours; } }

		public uint Minute { get { return _minutes; } }

		public ClockTime (uint hours, uint minutes)
		{
			_hours = hours;
			_minutes = minutes;

			if (hours > 23)
				throw new ArgumentException ("Value for 'hours' must be in the interval 0-23");

			if (minutes > 59)
				throw new ArgumentException ("Value for 'minutes' must be in the interval 0-59");
		}
	}

	public struct ClockTimeSpan
	{
		public ClockTime _start;
		public ClockTime _end;

		public ClockTime Start { get { return _start; } }

		public ClockTime End { get { return _end; } }

		public ClockTimeSpan (ClockTime start, ClockTime end)
		{
			_start = start;
			_end = end;
		}

		public bool IsInSpan (DateTime dt)
		{
			return (dt.Hour >= _start.Hour && dt.Minute >= _start.Minute) && (dt.Hour <= _end.Hour && dt.Minute <= _end.Minute);
		}

		public bool IsInSpan (ClockTime t)
		{
			return (t.Hour >= _start.Hour && t.Minute >= _start.Minute) && (t.Hour <= _end.Hour && t.Minute <= _end.Minute);
		}
	}
}

