using MasDev.Common.Injection;
using MasDev.Common.IO;

namespace MasDev.Common.Injection
{
    public sealed class WinRTDependencyConfigurator : IDependencyConfigurator
    {
        public void ConfigureDependencies(IDependencyContainer container)
        {
            container.AddDependency<IRegistryProvider, WinRtRegistryProvider>(LifeStyles.Singleton);
        }
    }
}
