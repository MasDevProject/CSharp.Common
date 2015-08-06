using System;
using System.Threading.Tasks;

namespace MasDev.Services.Auth
{
	public interface IIdentityRepository
	{
		Task<DateTime?> GetlastInvalidationUtcAsync (int id, int flag);

		Task SetInvalidationTime (int id, int flag, DateTime invalidationTimeUtc);
	}

	public interface ICachedIdentityRepository : IIdentityRepository
	{
		void ClearCache ();
	}
}

