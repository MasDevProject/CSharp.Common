using System;
using NodeExpress;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasDev.Extensions
{
	public static class ExpressExtensions
	{
        const int METHOD_GET = 0;
        const int METHOD_POST = 1;
        const int METHOD_PUT = 2;
        const int METHOD_DELETE = 3;

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

        private static ExpressHandler CreateExpressHandler(TaskCompletionSource<object> tcs, Func<ExpressServerRequest, ExpressServerResponse, Task> function)
	    {
            return delegate(ExpressServerRequest request, ExpressServerResponse response)
            {
                var task = function(request, response);
                task.GetAwaiter().OnCompleted(() =>
                {
                    if (task.Exception == null) tcs.SetResult(null);
                    else tcs.SetException(task.Exception);
                });
            };
	    }

        private static async Task HandleAsync(this ExpressApplication app, string path, int method, Func<ExpressServerRequest, ExpressServerResponse, Task> function)
        {
            var tcs = new TaskCompletionSource<object>();
            var handler = CreateExpressHandler(tcs, function);
            switch (method)
            {
                case METHOD_GET:
                    app.Get(path, handler);
                    break;
                case METHOD_POST:
                    app.Post(path, handler);
                    break;
                case METHOD_DELETE:
                    app.Delete(path, handler);
                    break;
                case METHOD_PUT:
                    app.Put(path, handler);
                    break;
                default:throw new ArgumentException("Method not mapped");
            }
            await tcs.Task;
        }

        public static async void GetAsync(this ExpressApplication app, string path, Func<ExpressServerRequest, ExpressServerResponse, Task> function)
        {
            await app.HandleAsync(path, METHOD_GET, function);
        }

        public static async void PostAsync(this ExpressApplication app, string path, Func<ExpressServerRequest, ExpressServerResponse, Task> function)
        {
            await app.HandleAsync(path, METHOD_POST, function);
        }

        public static async void DeleteAsync(this ExpressApplication app, string path, Func<ExpressServerRequest, ExpressServerResponse, Task> function)
        {
            await app.HandleAsync(path, METHOD_DELETE, function);
        }

        public static async void PutAsync(this ExpressApplication app, string path, Func<ExpressServerRequest, ExpressServerResponse, Task> function)
        {
            await app.HandleAsync(path, METHOD_PUT, function);
        }
	}

	class WrappedObject
	{
		public dynamic Content { get; set; }
	}
}

