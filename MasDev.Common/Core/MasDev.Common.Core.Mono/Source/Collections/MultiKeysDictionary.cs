using System.Collections.Generic;

namespace MasDev.Common
{
	public class MultiKeysDictionary<TKey1, TKey2, TValue> : ExclusiveAccessDictionary<TKey1, Dictionary<TKey2, TValue>>
	{
		public void Add (TKey1 key1, TKey2 key2, TValue value)
		{
			if (!ContainsKey (key1))
				Add (key1, new Dictionary<TKey2, TValue> ());

			var innerDict = this [key1];
			if (innerDict.ContainsKey (key2))
				innerDict [key2] = value;
			else
				innerDict.Add (key2, value);
		}

		public void Remove (TKey1 key1, TKey2 key2)
		{
			if (!ContainsKey (key1))
				return;

			var dict = this [key1];
			if (dict.ContainsKey (key2))
				dict.Remove (key2);
		}

		public bool ContainsKey (TKey1 key1, TKey2 key2)
		{
			return ContainsKey (key1) && this [key1].ContainsKey (key2);
		}

		public TValue this [TKey1 key1, TKey2 key2] {
			get { return ContainsKey (key1, key2) ? this [key1] [key2] : default(TValue); }
			set { Add (key1, key2, value); }
		}

		public override IIdentifier GetIdentifier (TKey1 key)
		{
			return new Identifier<TKey1, TValue> (key);
		}
	}
}