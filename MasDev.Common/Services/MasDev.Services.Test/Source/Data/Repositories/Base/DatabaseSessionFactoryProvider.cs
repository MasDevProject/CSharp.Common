using MasDev.Services.Test.Models;
using MasDev.Data.NHibernate.Providers;

namespace MasDev.Services.Test.Data
{
	public class DatabaseSessionFactoryProvider : NHibernateMySqlSessionFactoryProvider<ModelsMapper>
	{
		public DatabaseSessionFactoryProvider ()
			: base (typeof(User).Namespace, "127.0.0.1", "test", "test", "test", "web", true)
		{

		}
	}
}

