using System;
using System.Threading.Tasks;

namespace MasDev.Services.Auth
{
	public interface ICredentialsRepository
	{
		Task<DateTime?> GetlastInvalidationUtcAsync (int id, int flag);

		Task SetInvalidationTime (int id, int flag, DateTime invalidationTimeUtc);
	}

	public interface ICachedCredentialsRepository : ICredentialsRepository
	{
		void ClearCache ();
	}
}

