
namespace MasDev.Common
{
	public abstract class BaseViewModel<TConfigurator> : IViewModel where TConfigurator : IViewModelConfigurator
	{
		protected TConfigurator Configurator;

		public virtual void Initialize (TConfigurator configurator)
		{
			Configurator = configurator;
		}

		public virtual void SubscribeEvents () {}

		public virtual void UnsubscribeEvents () {}
	}
}

