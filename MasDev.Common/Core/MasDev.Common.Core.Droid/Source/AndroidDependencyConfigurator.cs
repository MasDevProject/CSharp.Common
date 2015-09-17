using MasDev.Security;
using MasDev.IO;
using Android.Content;
using MasDev.Patterns.Injection;
using MasDev.Utils;
using MasDev.Droid.IO;
using MasDev.Droid.Utils;

namespace MasDev.Droid
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
			container.AddDependency<ISymmetricCrypto, Aes> (LifeStyles.Singleton);
			container.AddDependency<IFileIO, FileIO> (LifeStyles.Singleton);
			container.AddDependency<IRegistryProvider> (() => new RegistryProvider (_context), LifeStyles.Singleton);
			container.AddDependency<ILogger, Logger> (LifeStyles.Singleton);
			container.AddDependency<ITimeZoneConverter, TimeZoneConverter> (LifeStyles.Singleton);
		}
	}
}
