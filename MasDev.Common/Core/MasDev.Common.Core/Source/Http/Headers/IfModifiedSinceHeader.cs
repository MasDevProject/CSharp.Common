using System;
using MasDev.Common.Http;
using System.Collections.Generic;
using MasDev.IO.Http;

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
                    // TODO FIXME HACK
		            TimeUtc = DateTime.ParseExact(timeUtc.Replace(" GMT", string.Empty), "ddd, dd MMM YYYY HH:mm:ss", null);
		        }
		        catch
		        {
		            TimeUtc = DateTime.MinValue;
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

