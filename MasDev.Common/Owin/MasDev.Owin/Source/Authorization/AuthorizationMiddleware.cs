using Microsoft.Owin;
using System.Threading.Tasks;


namespace MasDev.Owin.Middlewares
{
	public class AuthorizationMiddleware : OwinMiddleware
	{
		public AuthorizationMiddleware (OwinMiddleware next) : base (next)
		{
		}


		public override async Task Invoke (IOwinContext context)
		{
			// TODO implementation
			await Next.Invoke (context);
		}
	}
}

