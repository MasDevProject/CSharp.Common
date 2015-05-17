using MasDev.Patterns.Injection;
using MasDev.Services.Test.Communication;

namespace MasDev.Services.Test
{
	public partial class TestDependencyConfigurator
	{
		public void RegisterConsistencyValidators (IDependencyContainer container)
		{
			container.RegisterConsistencyValidator<UserDto, UserConsistencyValidator> ();
		}
	}
}

