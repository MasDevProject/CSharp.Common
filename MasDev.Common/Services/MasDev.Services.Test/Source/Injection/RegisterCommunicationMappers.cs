using MasDev.Patterns.Injection;
using MasDev.Services.Test.Communication;
using MasDev.Services.Test.Models;

namespace MasDev.Services.Test
{
	public partial class TestDependencyConfigurator
	{
		public void RegisterCommunicationMappers (IDependencyContainer container)
		{
			container.RegisterCommunicationMapper<UserDto, User, UserDtoMapper> ();
		}
	}
}

