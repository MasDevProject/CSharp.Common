using MasDev.Common.Data;
using MasDev.Common.Share.Tests.Models;

namespace MasDev.Common.Share.Tests
{
	public interface IInterestRepository : IRepository<Interest>
	{
	}

	public class InterestRepositorySQLite : SQLiteRepository<Interest>, IInterestRepository
	{
		public InterestRepositorySQLite (string connectionString) : base (connectionString)
		{

		}
	}
}

