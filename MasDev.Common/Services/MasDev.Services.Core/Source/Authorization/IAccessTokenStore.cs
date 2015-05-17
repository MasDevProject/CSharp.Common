using System;
using System.Threading.Tasks;

namespace MasDev.Services.Auth
{
	public interface IAccessTokenStore
	{
		Task<DateTime?> GetlastInvalidationUtcAsync (int id, int flag);

		Task SetInvalidationTime (int id, int flag, DateTime invalidationTimeUtc);
	}
}

