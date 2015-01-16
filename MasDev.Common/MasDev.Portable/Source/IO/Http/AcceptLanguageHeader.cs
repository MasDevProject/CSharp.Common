using System.Collections.Generic;
using System;


namespace MasDev.IO.Http
{
	public class AcceptLanguageHeader
	{
		public const string Name = "Accept-Language";



		public string Locale { get; set; }



		public decimal? EstimatePreference { get; set; }



		public override string ToString ()
		{

			return EstimatePreference.HasValue ?
				string.Format ("{0};q={1},", Locale, EstimatePreference.Value) :
				string.Format ("{0},", Locale);
		}



		public static IEnumerable<AcceptLanguageHeader> Parse (string headerValue)
		{
			var values = headerValue.Split (',');
			foreach (var value in values) {
				var parts = value.Trim ().Split (';');
				if (parts.Length == 0)
					throw new ArgumentException ("Parse error");

				var header = new AcceptLanguageHeader ();
				if (parts.Length == 1) {
					header.Locale = parts [0].Trim ();
					yield return header;
				}


				header.Locale = parts [0];
				header.EstimatePreference = decimal.Parse (parts [1].Replace ("q=", string.Empty));
			}
		}
	}
}

