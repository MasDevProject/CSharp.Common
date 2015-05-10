using System;
using MasDev.Owin.Auth;
using System.Threading.Tasks;

namespace MasDev.Owin.Test
{
	public class DummyAccessTokenStore : IAccessTokenStore
	{

		public async Task<DateTime?> GetlastInvalidationUtcAsync (int id, int flag)
		{
			return await Task.FromResult (DateTime.MinValue);
		}

		public Task SetInvalidationTime (int id, int flag, DateTime invalidationTimeUtc)
		{
			return Task.Delay (0);
		}
	}
}

