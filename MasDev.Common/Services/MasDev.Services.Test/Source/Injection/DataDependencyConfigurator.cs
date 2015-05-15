using MasDev.Patterns.Injection;
using MasDev.Services.Auth;
using MasDev.Data.NHibernate;
using MasDev.Services.Test.Data;
using MasDev.Data;

namespace MasDev.Services.Test
{
	public partial class TestDependencyConfigurator
	{
		static void ConfigureData (IDependencyContainer container)
		{
			container.AddDependency<ISessionFactoryProvider, DatabaseSessionFactoryProvider> (LifeStyles.Singleton);
			container.AddDependency<IUnitOfWork, UnitOfWork> (PerRequestLifestyle.Instance);
			container.AddDependency<IUsersRepository, UsersRepository> (PerRequestLifestyle.Instance);
			container.AddDependency<IAccessTokenStore, DummyAccessTokenStore> (PerRequestLifestyle.Instance);
		}
	}
}

