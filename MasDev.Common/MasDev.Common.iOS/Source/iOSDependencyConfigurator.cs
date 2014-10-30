using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinIOS;
using MasDev.Common.Injection;
using MasDev.Common.Utils;

namespace MasDev.Common.iOS
{
	public class iOSDependencyConfigurator : IDependencyConfigurator
	{
		public void ConfigureDependencies (IDependencyContainer container)
		{
			container.AddDependency<ISQLitePlatform, SQLitePlatformIOS> (LifeStyles.Singleton);
			container.AddDependency<ITimeZoneConverter, TimeZoneConverter> (LifeStyles.Singleton);
		}
	}
}
	