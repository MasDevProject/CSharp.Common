using MasDev.Patterns.Injection;
using MasDev.Services.Test.Communication;
using MasDev.Services.Modeling;
using MasDev.Services.Test.Models;

namespace MasDev.Services.Test
{
	public partial class TestDependencyConfigurator
	{
		static void ConfigureModeling (IDependencyContainer container)
		{
			container.AddDependency<DtoMapper<UserDto, User>, UserDtoMapper> (LifeStyles.Singleton);
		}
	}
}

