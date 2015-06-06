using System.Collections.Generic;

namespace MasDev.Common
{
	public class MultiValuesDictionary<TKey, TCollection, TValue> : ExclusiveAccessDictionary<TKey, TCollection> 
		where TCollection : ICollection<TValue>, new()
	{
		public void Add (TKey key, TValue value)
		{
			if (ContainsKey (key))
				this [key].Add (value);
			else {
				var collection = new TCollection ();
				collection.Add (value);
				base.Add (key, collection);
			}
		}

		public void Remove (TKey key, TValue value)
		{
			if (!ContainsKey (key))
				return;
			this [key].Remove (value);
		}

		public override IIdentifier GetIdentifier (TKey key)
		{
			return new Identifier<TKey, TValue> (key);
		}
	}
}

