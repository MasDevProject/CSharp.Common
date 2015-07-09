using System.Threading.Tasks;
using System;
using System.Collections.Concurrent;

namespace MasDev.Services.Auth
{
	public class CachedAccessTokenStore : ICredentialsRepository
	{
		const string _cacheKeyFormat = "{0},{1}";

		readonly ConcurrentDictionary<string, DateTime?> _cache;
		readonly ICredentialsRepository _persistentStore;

		public CachedAccessTokenStore (ICredentialsRepository persistentStore)
		{
			_persistentStore = persistentStore;
			_cache = new ConcurrentDictionary<string, DateTime?> ();
		}

		public async Task<DateTime?> GetlastInvalidationUtcAsync (int id, int flag)
		{
			var cacheKey = GetCacheKey (id, flag);
			DateTime? cachedInvalidation;

			if (_cache.ContainsKey (cacheKey) && _cache.TryGetValue (cacheKey, out cachedInvalidation))
				return cachedInvalidation;

			var lastInvalidation = await _persistentStore.GetlastInvalidationUtcAsync (id, flag);
			_cache.AddOrUpdate (cacheKey, lastInvalidation, (key, oldValue) => lastInvalidation);
			return lastInvalidation;
		}


		public async Task SetInvalidationTime (int id, int flag, DateTime invalidationTimeUtc)
		{
			var cacheKey = GetCacheKey (id, flag);
			_cache.AddOrUpdate (cacheKey, invalidationTimeUtc, (key, oldValue) => invalidationTimeUtc);
			await _persistentStore.SetInvalidationTime (id, flag, invalidationTimeUtc);
		}


		static string GetCacheKey (int id, int flags)
		{
			return string.Format (_cacheKeyFormat, id, flags);
		}
	}
}

