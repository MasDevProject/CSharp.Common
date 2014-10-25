using MasDev.Common.IO;
using MasDev.Common.Injection;


namespace MasDev.Common.Mono
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

