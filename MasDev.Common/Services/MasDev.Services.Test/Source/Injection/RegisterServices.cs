using MasDev.Patterns.Injection;
using MasDev.Services.Test.Services;
using MasDev.Services.Test.Communication;

namespace MasDev.Services.Test
{
	public partial class TestDependencyConfigurator
	{
		public void RegisterServices (IDependencyContainer container)
		{
			container.RegisterCrudService<UserDto, IUserService, UsersService> ();
		}
	}
}

