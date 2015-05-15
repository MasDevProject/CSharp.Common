using Microsoft.Owin;
using System.Threading.Tasks;
using MasDev.Patterns.Injection.SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace MasDev.Services
{
	public class SimpleInjectorMiddleware : OwinMiddleware
	{
		readonly SimpleInjectorContainer _container;

		public SimpleInjectorMiddleware (OwinMiddleware next, SimpleInjectorContainer container) : base (next)
		{
			_container = container;
		}


		public override async Task Invoke (IOwinContext context)
		{
			using (_container.BeginExecutionContextScope ()) {
				await Next.Invoke (context);
			}
		}
	}

}

