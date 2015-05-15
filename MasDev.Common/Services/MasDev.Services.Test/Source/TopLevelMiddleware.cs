using Microsoft.Owin;
using System.Threading.Tasks;
using System;
using MasDev.Services.Test.Services;
using MasDev.Services.Test.Communication;


namespace MasDev.Services.Test
{
	public class TopLevelMiddleware : OwinMiddleware
	{
		public TopLevelMiddleware (OwinMiddleware next) : base (next)
		{
		}

		public override async Task Invoke (IOwinContext context)
		{
			var requestPath = context.Request.Path.ToString ();
			Console.WriteLine ("================================== Called {0}", requestPath);

			try {
				var service = new UsersService (null);
				var user = new UserDto { Username = requestPath };
				user = await service.CreateAsync (user);

				var service2 = new UsersService (null);
			} catch (Exception e) {
				Console.WriteLine (e);
			}

			Console.WriteLine ("================================== Ended {0}", requestPath);
		}
	}
}

