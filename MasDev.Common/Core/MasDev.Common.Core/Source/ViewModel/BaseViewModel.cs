
namespace MasDev.Common
{
	public abstract class BaseViewModel<TConfigurator> : IViewModel where TConfigurator : IViewModelConfigurator
	{
		protected TConfigurator Configurator;

		public virtual void Initialize (TConfigurator configurator)
		{
			Configurator = configurator;
		}

		public abstract void SubscribeEvents ();

		public abstract void UnsubscribeEvents ();
	}
}

