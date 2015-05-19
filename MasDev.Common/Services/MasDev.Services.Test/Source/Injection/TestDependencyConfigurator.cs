using MasDev.Mono;
using MasDev.Patterns.Injection;
using Newtonsoft.Json.Serialization;
using MasDev.Newtonsoft.ContractResolvers;
using SimpleInjector;

namespace MasDev.Services.Test
{
	public partial class TestDependencyConfigurator : MonoDependecyConfigurator, IAppDependencyConfigurator
	{
		public override void ConfigureDependencies (IDependencyContainer container)
		{
			container.AddDependency<IContractResolver, NHibernateContractResolver> (Lifestyle.Singleton);
			this.RegisterAppComponents (container);
		}
	}
}

