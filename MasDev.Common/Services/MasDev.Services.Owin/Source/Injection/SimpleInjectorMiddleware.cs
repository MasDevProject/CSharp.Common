using Microsoft.Owin;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;
using SimpleInjector;

namespace MasDev.Services
{
	public class SimpleInjectorMiddleware : OwinMiddleware
	{
		public SimpleInjectorMiddleware (OwinMiddleware next) : base (next)
		{
		}


		public override async Task Invoke (IOwinContext context)
		{
			var scope = new Scope ();
			CallContext.SetData (PerRequestLifestyle.CallContextKey, scope);

			await Next.Invoke (context);
			scope.Dispose ();
		}
	}

}

