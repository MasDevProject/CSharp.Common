using MasDev.Patterns.Injection;

namespace MasDev.Services
{
	public interface IAppDependencyConfigurator : IDependencyConfigurator
	{
		void RegisterServices (IDependencyContainer container);

		void RegisterRepositories (IDependencyContainer container);

		void RegisterConsistencyValidators (IDependencyContainer container);

		void RegisterDataAccessValidators (IDependencyContainer container);

		void RegisterCommunicationMappers (IDependencyContainer container);
	}

	public static class AppDependencyConfiguratorExtensions
	{
		public static void RegisterAppComponents (this IAppDependencyConfigurator configurator, IDependencyContainer container)
		{
			configurator.RegisterRepositories (container);
			configurator.RegisterConsistencyValidators (container);
			configurator.RegisterCommunicationMappers (container);
			configurator.RegisterDataAccessValidators (container);
			configurator.RegisterServices (container);
		}
	}
}

