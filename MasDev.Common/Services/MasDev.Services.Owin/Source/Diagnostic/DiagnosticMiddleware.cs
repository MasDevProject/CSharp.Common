using Microsoft.Owin;
using System.Threading.Tasks;
using System.Diagnostics;
using System;

namespace MasDev.Services
{
	public class DiagnosticMiddleware : OwinMiddleware
	{
		const string Divider = "\n================================================================================";
		const string ResponseFormat = "Response {0} in {1} ms";

		public DiagnosticMiddleware (OwinMiddleware next) : base (next)
		{
		}


		public override async Task Invoke (IOwinContext context)
		{
			try {
				Debug.WriteLine (Divider);
				WriteMarker ();
				Debug.Write (context.Request.Method);
				Debug.Write (' ');
				Debug.WriteLine (context.Request.Path);
				WriteHeaders (context.Request.Headers);
				Debug.WriteLine ('\n');

				var stopwatch = Stopwatch.StartNew ();
				await Next.Invoke (context);
				stopwatch.Stop ();

				Debug.WriteLine ('\n');
				WriteMarker ();
				Debug.WriteLine (string.Format (ResponseFormat, context.Response.StatusCode, stopwatch.ElapsedMilliseconds));
				WriteHeaders (context.Response.Headers);
			} catch (Exception e) {
				Console.WriteLine (e);
			}
		}

		static void WriteHeaders (IHeaderDictionary headers)
		{
			if (headers == null)
				return;
			foreach (var header in headers) {
				WriteMarker ();
				Debug.Write (header.Key);
				Debug.Write (": ");
				foreach (var headerValue in header.Value) {
					Debug.Write (headerValue);
					Debug.Write (", ");
				}
				Debug.WriteLine (string.Empty);
			}
		}

		static void WriteMarker ()
		{
			Debug.Write ('#');
			Debug.Write (" ");
		}
	}
}

