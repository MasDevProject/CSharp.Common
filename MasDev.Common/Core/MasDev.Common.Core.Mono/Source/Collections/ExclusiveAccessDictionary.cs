using System;
using System.Collections.Generic;
using MasDev.Utils;

namespace MasDev.Common
{
	public abstract class ExclusiveAccessDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IExclusiveAccessProvider
	{
		readonly object _lock = new object ();

		public virtual IDisposable StartExclusiveAccess (IIdentifier identifier)
		{
			lock (_lock) {
				return new LockByIdMutex (identifier.Identify);
			}
		}

		public abstract IIdentifier GetIdentifier (TKey key);
	}

	class Identifier<TKey, TValue> : IIdentifier
	{
		const string IdentifierFormat = "{0}_{1}_{2}s";
		readonly string _keyTypeName = typeof(TKey).Name;
		readonly string _valueTypeName = typeof(TValue).Name;
		readonly TKey _keyInstance;

		public Identifier (TKey keyInstance)
		{
			_keyInstance = keyInstance;
		}

		public string Identify {
			get { return string.Format (IdentifierFormat, _keyTypeName, _keyInstance, _valueTypeName); }
		}
	}
}

