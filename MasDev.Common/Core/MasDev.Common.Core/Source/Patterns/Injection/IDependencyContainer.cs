using System;


namespace MasDev.Patterns.Injection
{
	public interface IDependencyContainer
	{
		TInterface Resolve<TInterface> () where TInterface : class;

		void AddDependency<TInterface, TImplementation> () where TInterface : class where TImplementation : class, TInterface, new();

		void AddDependency<TInterface, TImplementation> (object lifeStyle) where TInterface : class where TImplementation : class, TInterface, new();

		void AddDependency<TInterface> (Func<TInterface> instantiator) where TInterface : class;

		void AddDependency<TInterface> (Func<TInterface> instantiator, object lifeStyle) where TInterface : class;
	}

}

