using System;
using MasDev.Common.Http;
using System.Collections.Generic;
using MasDev.IO.Http;
using System.Globalization;

namespace MasDev.Common
{
	public class IfModifiedSinceHeader : IHeader
	{
		public string Name { get { return Headers.IfModifiedSince; } }

		public DateTime TimeUtc { get; private set; }

		public IfModifiedSinceHeader (string timeUtc)
		{
		    try
		    {
		        TimeUtc = DateTime.Parse(timeUtc).ToUniversalTime();
		    }
		    catch
		    {
		        try
		        {
                    TimeUtc = DateTime.ParseExact(timeUtc, "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AdjustToUniversal); //Sat, 14 Mar 2015 10:09:09 GMT
		        }
		        catch
		        {
		            try
		            {
		                TimeUtc = DateTime.ParseExact(timeUtc, "ddd, dd MMM yyyy HH:mm:ss GMT", CultureInfo.InvariantCulture);
		            }
		            catch
		            {
		                TimeUtc = DateTime.MinValue;
		            }
		        }
		    }
		}

		public override string ToString ()
		{
			return TimeUtc.ToString ("R");
		}

		public IEnumerable<string> Values {
			get {
				yield return ToString ();
			}
		}
	}
}

