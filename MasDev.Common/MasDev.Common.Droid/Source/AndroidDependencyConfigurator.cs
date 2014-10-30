using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;
using MasDev.Common.Security;
using MasDev.Common.Security.Encryption;
using MasDev.Common.IO;
using MasDev.Common.Droid.IO;
using Android.Content;
using MasDev.Common.Injection;
using MasDev.Common.Droid.Utils;
using MasDev.Common.Utils;

namespace MasDev.Common.Droid
{
	public class AndroidDependencyConfigurator : IDependencyConfigurator
	{
		readonly Context _context;

		public AndroidDependencyConfigurator(Context ctx) 
		{
			_context = ctx;
		}

		public void ConfigureDependencies (IDependencyContainer container)
		{
			container.AddDependency<ISQLitePlatform, SQLitePlatformAndroid> (LifeStyles.Singleton);
			container.AddDependency<ISymmetricCrypto, Aes> (LifeStyles.Singleton);
			container.AddDependency<IFileIO, FileIO> (LifeStyles.Singleton);
			container.AddDependency<IRegistryProvider> (() => new RegistryProvider (_context), LifeStyles.Singleton);
			container.AddDependency<ILogger, Logger> (LifeStyles.Singleton);
			container.AddDependency<ITimeZoneConverter, TimeZoneConverter> (LifeStyles.Singleton);
		}
	}
}
