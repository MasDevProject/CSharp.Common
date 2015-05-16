using System;
using System.Web.Http;
using System.Net.Http;
using MasDev.Services.Middlewares;

namespace MasDev.Services.Owin.WebApi
{
	public class BaseApiController : ApiController
	{
		protected IIdentityContext IdentityContext { get { return Request.GetOwinContext ().Get<IIdentityContext> (AuthorizationMiddleware.IdentityContextKey); } }
	}
}

