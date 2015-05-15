using Microsoft.Owin;
using System.Threading.Tasks;

namespace MasDev.Services
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

