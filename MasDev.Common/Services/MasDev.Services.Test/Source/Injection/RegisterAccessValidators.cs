using MasDev.Patterns.Injection;
using MasDev.Services.Test.Communication;
using MasDev.Services.Test.Models;

namespace MasDev.Services.Test
{
	public partial class TestDependencyConfigurator
	{
		public void RegisterAccessValidators (IDependencyContainer container)
		{
			container.RegisterAccessValidator<UserDto, UserDtoAccessValidator> ();
			container.RegisterAccessValidator<User, UserAccessValidator> ();
		}
	}
}

