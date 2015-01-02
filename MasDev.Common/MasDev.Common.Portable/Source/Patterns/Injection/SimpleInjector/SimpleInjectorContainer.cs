using System;
using MasDev.Patterns.Injection;
using SimpleInjector;


namespace MasDev.Patterns.Injection.SimpleInjector
{
	public class SimpleInjectorContainer : Container, IDependencyContainer
	{
		public TInterface Resolve<TInterface> () where TInterface : class
		{
			return GetInstance<TInterface> ();
		}



		public void AddDependency<TInterface, TImplementation> ()
            where TInterface : class
            where TImplementation : class, TInterface, new()
		{
			Register<TInterface, TImplementation> ();
		}



		public void AddDependency<TInterface, TImplementation> (object lifeStyle)
            where TInterface : class
            where TImplementation : class, TInterface, new()
		{
			Register<TInterface, TImplementation> (EnsureIsSimpleInjector (lifeStyle));
		}



		public void AddDependency<TInterface> (Func<TInterface> instantiator) where TInterface : class
		{
			Register<TInterface> (instantiator);
		}



		public void AddDependency<TInterface> (Func<TInterface> instantiator, object lifeStyle) where TInterface : class
		{
			Register<TInterface> (instantiator, EnsureIsSimpleInjector (lifeStyle));
		}



		static Lifestyle EnsureIsSimpleInjector (object lifeStyle)
		{
			var l = lifeStyle as Lifestyle;
			if (l == null)
				throw new ArgumentException ("In order to use this container lifeStyle must be a subclass of SimpleInjector.Lifestyle");
			return l;
		}
	}
}

