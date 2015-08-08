using System;
using System.Collections.Generic;

namespace MasDev.Common
{
    public abstract class ExclusiveAccessDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IExclusiveAccessProvider
    {
        readonly object _lock = new object();

        public virtual IDisposable StartExclusiveAccess(IIdentifier identifier)
        {
            lock (_lock)
            {
                //return new LockByIdMutex (identifier.Identify); TODO scoprire perchè esplode
                return new DummyDisposable();
            }
        }

        public abstract IIdentifier GetIdentifier(TKey key);

        class DummyDisposable : IDisposable
        {
            public void Dispose()
            {

            }
        }
    }

    class Identifier<TKey, TValue> : IIdentifier
    {
        const string IdentifierFormat = "{0}_{1}_{2}s";
        readonly string _keyTypeName = typeof(TKey).Name;
        readonly string _valueTypeName = typeof(TValue).Name;
        readonly TKey _keyInstance;

        public Identifier(TKey keyInstance)
        {
            _keyInstance = keyInstance;
        }

        public string Identify
        {
            get { return string.Format(IdentifierFormat, _keyTypeName, _keyInstance, _valueTypeName); }
        }
    }
}

