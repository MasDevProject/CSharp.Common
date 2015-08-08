using MasDev.Services.Modeling;
using System.Collections.Generic;
using MasDev.Patterns.Injection;
using System.Linq;

namespace MasDev.Common
{
	public interface ICallingContext
	{
		Identity Identity { get; set; }

		int? Scope { get; set; }

		string Language { get; set; }

		string RequestPath { get; set; }

		string RequestIp { get; set; }

		string RequestHost { get; set; }

		MultiValueDictionary<string, string> RequestHeaders { get; set; }

		MultiValueDictionary<string, string> ResponseHeaders { get; set; }
	}

	public class MultiValueDictionary<TKey, TValue> : Dictionary<TKey, ISet<TValue>>
	{
		public void AddValue (TKey key, TValue value)
		{
			if (!ContainsKey (key))
				Add (key, new HashSet<TValue> ());

			this [key].Add (value);
		}

		public void AddValues (TKey key, params TValue[] values)
		{
			foreach (var value in values)
				AddValue (key, value);
		}

		public MultiValueDictionary (IDictionary<TKey, IEnumerable<TValue>> dict)
		{
			foreach (var key in dict.Keys) {
				AddValues (key, dict [key].ToArray ());
			}
		}

		public MultiValueDictionary ()
		{
			
		}
	}
}