using System.Threading.Tasks;
using System;
using System.Collections.Concurrent;

namespace MasDev.Services.Auth
{
    public class CachedIdentityRepository : ICachedIdentityRepository
    {
        const string _cacheKeyFormat = "{0},{1}";

        static readonly ConcurrentDictionary<string, DateTime?> _cache = new ConcurrentDictionary<string, DateTime?>();
        readonly IIdentityRepository _persistentRepository;

        public CachedIdentityRepository(IIdentityRepository persistentRepository)
        {
            _persistentRepository = persistentRepository;
        }

        public async Task<DateTime?> GetlastInvalidationUtcAsync(int id, int flag)
        {
            var cacheKey = GetCacheKey(id, flag);
            DateTime? cachedInvalidation;

            if (_cache.ContainsKey(cacheKey) && _cache.TryGetValue(cacheKey, out cachedInvalidation))
                return cachedInvalidation;

            var lastInvalidation = await _persistentRepository.GetlastInvalidationUtcAsync(id, flag);
            _cache.AddOrUpdate(cacheKey, lastInvalidation, (key, oldValue) => lastInvalidation);
            return lastInvalidation;
        }

        public async Task SetInvalidationTime(int id, int flag, DateTime invalidationTimeUtc)
        {
            var cacheKey = GetCacheKey(id, flag);
            _cache.AddOrUpdate(cacheKey, invalidationTimeUtc, (key, oldValue) => invalidationTimeUtc);
            await _persistentRepository.SetInvalidationTime(id, flag, invalidationTimeUtc);
        }

        static string GetCacheKey(int id, int flags)
        {
            return string.Format(_cacheKeyFormat, id, flags);
        }

        public void ClearCache()
        {
            _cache.Clear();
        }
    }
}

