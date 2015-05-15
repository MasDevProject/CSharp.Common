using MasDev.Mono;
using MasDev.Patterns.Injection;

namespace MasDev.Services.Test
{
	public partial class TestDependencyConfigurator : MonoDependecyConfigurator
	{
		public override void ConfigureDependencies (IDependencyContainer container)
		{
			ConfigureModeling (container);
			ConfigureData (container);
		}
	}
}

