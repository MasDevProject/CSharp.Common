using MasDev.IO;
using MasDev.Patterns.Injection;


namespace MasDev.Mono
{
	public class MonoDependecyConfigurator : IDependencyConfigurator
	{
		public virtual void ConfigureDependencies (IDependencyContainer container)
		{
			container.AddDependency<IRegistryProvider, RegistryProvider> (LifeStyles.Singleton);
			container.AddDependency<IPortableConsole, PortableConsole> (LifeStyles.Singleton);
		}
	}
}

