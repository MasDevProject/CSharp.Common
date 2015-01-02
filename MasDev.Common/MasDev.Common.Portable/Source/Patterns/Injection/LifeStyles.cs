using SimpleInjector;


namespace MasDev.Patterns.Injection
{
	public static class LifeStyles
	{
		public static readonly object Singleton = Lifestyle.Singleton;
		public static readonly object Transient = Lifestyle.Transient;
	}
}

