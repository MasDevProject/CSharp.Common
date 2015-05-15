using System;
using System.Threading.Tasks;
using MasDev.Services.Auth;

namespace MasDev.Services
{
	public class DummyAccessTokenStore : IAccessTokenStore, IDisposable
	{
		public DummyAccessTokenStore ()
		{
			Console.WriteLine ("DummyAccessTokenStore instantiated");
		}

		public async Task<DateTime?> GetlastInvalidationUtcAsync (int id, int flag)
		{
			return await Task.FromResult (DateTime.MinValue);
		}

		public Task SetInvalidationTime (int id, int flag, DateTime invalidationTimeUtc)
		{
			return Task.Delay (0);
		}

		public void Dispose ()
		{
			Console.WriteLine ("DummyAccessTokenStore disposed");
		}
	}
}

