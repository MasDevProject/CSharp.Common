using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace MasDev.Rest.WebApi
{
	public class DynamicDictionary
	{
		readonly Dictionary<string, string> _innerDictionary = new Dictionary<string, string> ();



		public T Get<T> (string key)
		{
			var json = this [key];
			if (json == null)
				return default(T);
			try {
				return JsonConvert.DeserializeObject<T> (json);
			} catch (Exception e) {
				return default(T);
			}
		}



		public string this [string i] {
			get { 
				try {
					return _innerDictionary [i];
				} catch (Exception) {
					return null;
				}
			}
			set { _innerDictionary [i] = value; }
		}



		internal void Add (string key, string value)
		{
			if (key == null || value == null)
				return;

			_innerDictionary.Add (key, value);
		}
	}
}

