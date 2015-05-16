using MasDev.Patterns.Injection;
using MasDev.Services.Test.Communication;
using MasDev.Services.Test.Services;

namespace MasDev.Services.Test
{
	public partial class TestDependencyConfigurator
	{
		static void ConfigureServices (IDependencyContainer container)
		{
			container.AddDependency<ICrudService<UserDto>, UsersService> (LifeStyles.Singleton);
		}
	}
}

