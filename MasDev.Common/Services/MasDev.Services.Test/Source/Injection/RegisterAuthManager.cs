using MasDev.Patterns.Injection;
using MasDev.Services.Auth;

namespace MasDev.Services.Test
{
	public partial class TestDependencyConfigurator
	{
		public void RegisterAuthManager (IDependencyContainer container)
		{
			container.RegisterAuthManager (CreateAuthManager);
		}

		static IAuthorizationManager CreateAuthManager ()
		{
			var pipeline = new DefaultAccessTokenPipeline ("pwd");
			return new AuthorizationManager (pipeline, Injector.Resolve<IAccessTokenStore>);
		}
	}
}

