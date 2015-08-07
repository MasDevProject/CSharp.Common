using Microsoft.Owin;
using System.Threading.Tasks;
using System.Diagnostics;
using System;

namespace MasDev.Services
{
	public class DiagnosticMiddleware : OwinMiddleware
	{
        static readonly object _lock = new object();
		const string Divider = "\n================================================================================";
		const string RequestFormat = "REQUEST: {0} @ {1}";
		const string ResponseFormat = "RESULT : {0} in {1} ms";
		ConsoleColor? _consoleColor;

		public DiagnosticMiddleware (OwinMiddleware next) : base (next)
		{
		}

		public override async Task Invoke (IOwinContext context)
		{
			if (!_consoleColor.HasValue)
				_consoleColor = Console.ForegroundColor;
			
			Exception e = null;
			var stopwatch = Stopwatch.StartNew ();
			try {
				await Next.Invoke (context);
			} catch (Exception ex) {
				e = ex;
			}
			stopwatch.Stop ();

            lock (_lock)
            {
                Console.ForegroundColor = e == null ? _consoleColor.Value : ConsoleColor.Red;
                Console.WriteLine(Divider);
                Console.WriteLine(string.Format(RequestFormat, context.Request.Method, context.Request.Path));
                Console.WriteLine(string.Format(ResponseFormat, context.Response.StatusCode, stopwatch.ElapsedMilliseconds));
                if (e != null)
                    Console.WriteLine(e);
                Console.WriteLine();
            }
		}

	}
}

