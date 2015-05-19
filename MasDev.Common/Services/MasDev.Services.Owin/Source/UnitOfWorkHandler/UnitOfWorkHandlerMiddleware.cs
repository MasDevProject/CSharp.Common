using Microsoft.Owin;
using System.Threading.Tasks;
using MasDev.Patterns.Injection;
using MasDev.Data;
using System.Linq;
using System.Diagnostics;

namespace MasDev.Services.Middlewares
{

	public class UnitOfWorkHandlerMiddleware : OwinMiddleware
	{
		const string RolledBack = "Unit of work rolled back";
		const string Committed = "Unit of work committed";

		static readonly int[] _ok = { 200, 201, 204, 304 };
		readonly int[] _committableStatusCodes;

		public UnitOfWorkHandlerMiddleware (OwinMiddleware next, params int[] committableStatusCodes) : base (next)
		{
			_committableStatusCodes = committableStatusCodes == null ?
				_ok : 
				committableStatusCodes.Concat (_ok).ToArray ();
		}


		public override async Task Invoke (IOwinContext context)
		{
			try {
				await Next.Invoke (context);
				HandleUow (context, false);
			} catch {
				HandleUow (context, true);
				throw;
			}
		}

		void HandleUow (IOwinContext context, bool rollback)
		{
			var uow = Injector.Resolve<IUnitOfWork> ();
			if (uow == null || !uow.IsStarted)
				return;
			
			if (rollback || _committableStatusCodes.All (c => context.Response.StatusCode != c)) {
				uow.Rollback (false);
				Debug.WriteLine (RolledBack);
			} else {
				uow.Commit (false);
				Debug.WriteLine (Committed);
			}
				
		}
	}
}

