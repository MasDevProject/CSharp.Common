using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinIOS;
using MasDev.Patterns.Injection;
using MasDev.Utils;
using MasDev.Common;
using MasDev.IO;
using MasDev.Security;

namespace MasDev.iOS
{
	public class iOSDependencyConfigurator : IDependencyConfigurator
	{
		public void ConfigureDependencies (IDependencyContainer container)
		{
			container.AddDependency<ISQLitePlatform, SQLitePlatformIOS> (LifeStyles.Singleton);
			container.AddDependency<ISymmetricCrypto, Aes> (LifeStyles.Singleton);
			container.AddDependency<IFileIO, FileIO> (LifeStyles.Singleton);
			container.AddDependency<IRegistry, Registry> (LifeStyles.Singleton);
			container.AddDependency<ILogger, Logger> (LifeStyles.Singleton);
			container.AddDependency<ITimeZoneConverter, TimeZoneConverter> (LifeStyles.Singleton);
		}
	}
}
	