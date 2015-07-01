using System.Web.Http;
using MasDev.Patterns.Injection;

namespace MasDev.Services.Owin.WebApi
{
	public class ServiceApiController : ApiController
	{
	}

	public class ServiceApiController<TService> : ServiceApiController where TService : class, IService
	{
		protected readonly TService Service;

		public ServiceApiController ()
		{
			Service = Injector.Resolve<TService> ();
		}
	}
}

