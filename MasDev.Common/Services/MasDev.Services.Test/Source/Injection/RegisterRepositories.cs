using MasDev.Patterns.Injection;
using MasDev.Data.NHibernate;
using MasDev.Services.Test.Data;
using MasDev.Services.Test.Models;

namespace MasDev.Services.Test
{
	public partial class TestDependencyConfigurator
	{
		public void RegisterRepositories (IDependencyContainer container)
		{
			container.AddDependency<ISessionFactoryProvider, DatabaseSessionFactoryProvider> (LifeStyles.Singleton);
			container.RegisterUnitOfWork<UnitOfWork> ();
			container.RegisterAccessTokenStore<DummyAccessTokenStore> ();

			container.RegisterRepository<User, IUsersRepository, UsersRepository> ();

		}
	}
}

