using MasDev.Patterns.Injection;
using MasDev.Services.Test.Communication;
using MasDev.Services.Test.Models;
using MasDev.Services.Modeling;

namespace MasDev.Services.Test
{
	public partial class TestDependencyConfigurator
	{
		static void ConfigureModeling (IDependencyContainer container)
		{
			container.AddDependency<ICommunicationMapper<UserDto, User>, UserDtoMapper> (LifeStyles.Singleton);
			container.AddDependency<IValidator<UserDto>, UserValidator> (LifeStyles.Singleton);
		}
	}
}

