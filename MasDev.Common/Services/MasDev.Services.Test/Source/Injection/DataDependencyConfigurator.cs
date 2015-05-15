using MasDev.Patterns.Injection;
using MasDev.Services.Auth;

namespace MasDev.Services.Test
{
	public partial class TestDependencyConfigurator
	{
		static void ConfigureData (IDependencyContainer container)
		{
			container.AddDependency<IAccessTokenStore, DummyAccessTokenStore> (PerRequestLifestyle.Instance);
		}
	}
}

