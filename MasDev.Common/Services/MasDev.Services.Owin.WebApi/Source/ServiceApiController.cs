using System.Web.Http;
using System.Net.Http;
using MasDev.Services.Middlewares;
using MasDev.Patterns.Injection;

namespace MasDev.Services.Owin.WebApi
{
	public class ServiceApiController : ApiController
	{
		protected IIdentityContext IdentityContext { get { return Request.GetOwinContext ().Get<IIdentityContext> (AuthorizationMiddleware.IdentityContextKey); } }
	}

	public class ServiceApiController<TService> : ServiceApiController where TService : IService
	{
		protected readonly TService Service;

		public ServiceApiController ()
		{
			Service = Injector.Resolve<TService> ();
		}
	}
}

