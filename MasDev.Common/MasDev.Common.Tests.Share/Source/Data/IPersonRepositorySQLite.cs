using MasDev.Common.Data;
using MasDev.Common.Share.Tests.Models;

namespace MasDev.Common.Share.Tests
{
	public interface IPersonRepository : IRepository<Person>
	{
	}

	public class IPersonRepositorySQLite : SQLiteRepository<Person>, IPersonRepository
	{
		public IPersonRepositorySQLite (string connectionString) : base (connectionString)
		{
		}
	}
}
