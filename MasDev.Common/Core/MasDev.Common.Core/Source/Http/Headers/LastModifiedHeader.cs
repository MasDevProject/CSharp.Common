using System;
using MasDev.Common.Http;
using System.Collections.Generic;
using MasDev.IO.Http;

namespace MasDev.Common
{
	public class LastModifiedHeader : IHeader
	{
		public string Name { get { return Headers.LastModified; } }

		readonly DateTime _timeUtc;

		public LastModifiedHeader (DateTime timeUtc)
		{
			_timeUtc = timeUtc;
		}

		public override string ToString ()
		{
			return _timeUtc.ToString ("R");
		}

		public IEnumerable<string> Values {
			get {
				yield return ToString ();
			}
		}
	}
}

