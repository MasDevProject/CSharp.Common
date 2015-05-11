using Microsoft.Owin;
using System.Threading.Tasks;

namespace MasDev.Common.Owin.Middlewares
{
	public class CompressionMiddleware : OwinMiddleware
	{
		public CompressionMiddleware (OwinMiddleware next) : base (next)
		{
		}


		public override async Task Invoke (IOwinContext context)
		{
			// TODO implementation
			await Next.Invoke (context);
		}
	}
}

