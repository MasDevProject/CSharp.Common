using NodeExpress;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasDev.Extensions
{
	public static class ExpressExtensions
	{
		public static void EnableCors (this ExpressApplication app, string host, IEnumerable<string> acceptedHeaders)
		{
			if (acceptedHeaders == null)
				acceptedHeaders = new List<string> ();

			var acceptedHeadersString = acceptedHeaders
				.Concat ("Origin")
				.Concat ("X-Requested-With")
				.Concat ("Content-Type")
				.Concat ("Accept")
				.Concat ("Authorization")
				.Distinct ()
				.Aggregate ((acc, current) => acc == null ? (current + ", ") : (acc + current + ", "));
			acceptedHeadersString = acceptedHeadersString.Substr (0, acceptedHeadersString.Length - 2);

			app.Use ((req, res, next) => {
				res.SetHeader ("Access-Control-Allow-Origin", host);
				res.SetHeader ("Access-Control-Allow-Headers", acceptedHeadersString);
				res.SetHeader ("Access-Control-Allow-Methods", "POST, PUT, GET, DELETE");
				next ();
			});
		}

		public static Task<dynamic> ListenAsync (this ExpressApplication app, int port)
		{
			var tcs = new TaskCompletionSource<dynamic> ();
			var server = new WrappedObject ();
			server.Content = app.Listen (port, () => tcs.SetResult (server.Content));
			return tcs.Task;
		}
	}

	class WrappedObject
	{
		public dynamic Content { get; set; }
	}
}

