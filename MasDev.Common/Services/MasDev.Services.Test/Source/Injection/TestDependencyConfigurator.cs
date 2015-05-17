using MasDev.Mono;
using MasDev.Patterns.Injection;

namespace MasDev.Services.Test
{
	public partial class TestDependencyConfigurator : MonoDependecyConfigurator, IAppDependencyConfigurator
	{
		public override void ConfigureDependencies (IDependencyContainer container)
		{
			this.RegisterAppComponents (container);
		}
	}
}

