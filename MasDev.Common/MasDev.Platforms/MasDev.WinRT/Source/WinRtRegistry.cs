using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Foundation.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MasDev.Common.IO
{
    public sealed class WinRtRegistryProvider : IRegistryProvider
    {
        public IRegistry GetRegistry(string registryName)
        {
            return new WinRtRegistry(registryName);
        }
    }

    sealed class WinRtRegistry : IRegistry
    {
        readonly string _registryName;
        IPropertySet _registry;

        readonly IDictionary<string, object> _added = new Dictionary<string, object>();
        readonly ISet<string> _removed = new HashSet<string>();

        public WinRtRegistry(string registryName)
        {
            _registryName = registryName;
        }


        public void Put(string key, object value)
        {
            _added[key] = value;
        }

        public T Read<T>(string key, T defaultValue)
        {
            if (!_registry.ContainsKey(key)) return defaultValue;
            return (T)_registry[key];
        }

        public void Remove(string key)
        {
            _removed.Add(key);
        }

        public void Prepare()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (!localSettings.Containers.ContainsKey(_registryName))
                localSettings.CreateContainer(_registryName, ApplicationDataCreateDisposition.Always);
            _registry = localSettings.Containers[_registryName].Values;
        }

        public Task PrepareAsync()
        {
            return Task.Run(() => Prepare());
        }

        public void Commit()
        {
            foreach (var added in _added)
                _registry[added.Key] = added.Value;
            _added.Clear();

            foreach (var removed in _removed)
                _registry.Remove(removed);
            _removed.Clear();
        }

        public Task CommitAsync()
        {
            return Task.Run(() => Commit());
        }

        public void Rollback()
        {
            _added.Clear();
            _removed.Clear();
        }

        public void Clear()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Containers.ContainsKey(_registryName))
                localSettings.DeleteContainer(_registryName);
            localSettings.CreateContainer(_registryName, ApplicationDataCreateDisposition.Always);
            _registry = localSettings.Containers[_registryName].Values;
        }

        public RegistryConfiguration Configuration
        {
            set { throw new NotSupportedException("Registry configuration is not supported yet"); }
        }

        public IEnumerable<string> Keys
        {
            get { return _registry.Keys; }
        }
    }
}
