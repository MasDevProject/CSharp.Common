using System;


namespace MasDev.Common.Injection
{
	public static class Injector
	{
		static IDependencyContainer _container;



		public static void InitializeWith (IDependencyContainer container, params IDependencyConfigurator[] configurators)
		{
			_container = container;
			if (configurators == null)
				return;

			foreach (var config in configurators)
				config.ConfigureDependencies (_container);
		}



		public static TInterface Resolve<TInterface> () where TInterface : class
		{
			EnsureIsInitialized ();
			return _container.Resolve <TInterface> ();
		}



		public static  void AddDependency<TInterface, TImplementation> () where TInterface : class where TImplementation : class, TInterface, new()
		{
			EnsureIsInitialized ();
			_container.AddDependency<TInterface, TImplementation> ();
		}



		public static void AddDependency<TInterface, TImplementation> (object lifeStyle) where TInterface : class where TImplementation : class, TInterface, new()
		{
			EnsureIsInitialized ();
			_container.AddDependency<TInterface, TImplementation> (lifeStyle);
		}



		public static void AddDependency<TInterface> (Func<TInterface> instantiator) where TInterface : class
		{
			EnsureIsInitialized ();
			_container.AddDependency<TInterface> (instantiator);
		}



		public static void AddDependency<TInterface> (Func<TInterface> instantiator, object lifeStyle) where TInterface : class
		{
			EnsureIsInitialized ();
			_container.AddDependency<TInterface> (instantiator, lifeStyle);
		}



		static void EnsureIsInitialized ()
		{
			if (_container == null)
				throw new NotSupportedException ("You must initialize te injector first");
		}

	}
}

