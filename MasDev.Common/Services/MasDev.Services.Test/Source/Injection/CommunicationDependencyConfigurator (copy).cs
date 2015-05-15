using MasDev.Patterns.Injection;
using MasDev.Services.Test.Communication;

namespace MasDev.Services.Test
{
	public partial class TestDependencyConfigurator
	{
		static void ConfigureModeling (IDependencyContainer container)
		{
			container.AddDependency<UserDtoMapper, UserDtoMapper> (LifeStyles.Singleton);
		}
	}
}

