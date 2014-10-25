using MasDev.Common.Dependency;

namespace MasDev.Common.Share.Tests
{
	public class CoreDependencyConfigurator : IDependencyConfigurator
	{
		readonly string _connectionString;

		public CoreDependencyConfigurator (string connectionString)
		{
			_connectionString = connectionString;
		}

		public void ConfigureDependencies (IDependencyContainer container)
		{
			container.AddDependency<IPersonRepository, IPersonRepositorySQLite> (LifeTimes.Singleton, () => new IPersonRepositorySQLite (_connectionString));
		}
	}
}
