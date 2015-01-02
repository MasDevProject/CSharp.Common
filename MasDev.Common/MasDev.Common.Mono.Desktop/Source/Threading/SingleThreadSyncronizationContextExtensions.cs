using System;
using System.Threading.Tasks;
using System.Threading;

namespace MasDev.Common.Threading
{
	public static class SingleThreadSyncronizationContextExtensions
	{

		public static Task SendAsync (this SynchronizationContext syncCtx, Action action)
		{
			var tcs = new TaskCompletionSource<bool> ();
			syncCtx.Post (dummy => {
				try {
					action ();
					tcs.TrySetResult (true);
				} catch (Exception ex) {
					tcs.TrySetException (ex);
				}
			}, null);
			return tcs.Task;
		}

		public static Task<T> SendAsync<T> (this SynchronizationContext syncCtx, Func<T> func)
		{
			var tcs = new TaskCompletionSource<T> ();
			syncCtx.Post (dummy => {
				try {
					var result = func ();
					tcs.TrySetResult (result);
				} catch (Exception ex) {
					tcs.TrySetException (ex);
				}
			}, null);
			return tcs.Task;
		}

		public static void Post (this SynchronizationContext ctx, Action action)
		{
			ctx.Post (dummy => action (), null);
		}

		public static void Send (this SynchronizationContext ctx, Action action)
		{
			ctx.Send (dummy => action (), null);
		}

		public static void Post (this SynchronizationContext ctx, Func<Task> func)
		{
			ctx.Post (dummy => func (), null);

		}

		public static void Send (this SynchronizationContext ctx, Func<Task> func)
		{
			var t = ctx.SendAsync (func);
			try {
				t.Wait ();
			} catch (AggregateException ex) {
				if (ex.InnerException != null)
					throw ex.InnerException;
				throw;
			}

		}

		public static Task SendAsync (this SynchronizationContext ctx, Func<Task> func)
		{
			var tcs = new TaskCompletionSource<bool> ();
			ctx.Post (async dummy => {
				try {
					await func ();
					tcs.TrySetResult (true);
				} catch (Exception ex) {
					tcs.TrySetException (ex);
				}

			}, null);
			return tcs.Task;
		}
	}
}

