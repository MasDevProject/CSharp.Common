using System;
using System.Threading.Tasks;


namespace MasDev.Utils
{
	public interface ISimpleAwaitableEvent<T>
	{
		event Action<T> Done;
	}





	public interface ISimpleAwaitableEvent
	{
		event Action Done;
	}





	public interface IAwaitableEvent<T> : ISimpleAwaitableEvent<T>
	{
		event Action<Exception> Failed;
		event Action Aborted;
	}





	public interface IAwaitableEvent
	{
		event Action Done;
		event Action<Exception> Failed;
		event Action Aborted;
	}





	public static class AwaitableEvent
	{
		public static Task<T> Await<T> (ISimpleAwaitableEvent<T> evt)
		{

			var tcs = new TaskCompletionSource<T> ();
			var raised = false;
			evt.Done += t => {
				if (raised)
					return;
				raised = true;
				tcs.SetResult(t);
			};
			return tcs.Task;
		}



		public static Task<T> AsTask<T> (this ISimpleAwaitableEvent<T> evt)
		{
			return Await (evt);
		}



		public static Task Await (ISimpleAwaitableEvent evt)
		{
			var tcs = new TaskCompletionSource<object> ();
			var raised = false;
			evt.Done += () => {
				if (raised)
					return;
				raised = true;
				tcs.SetResult (null);
			};
			return tcs.Task;
		}



		public static Task AsTask (this ISimpleAwaitableEvent evt)
		{
			return Await (evt);
		}



		public static Task<T> Await<T> (IAwaitableEvent<T> evt)
		{
			var tcs = new TaskCompletionSource<T> ();

			var doneRaised = false;
			var failedRaised = false;
			var abortedRaised = false;

			evt.Done += t => {
				if (doneRaised)
					return;
				doneRaised = true;
				tcs.SetResult (t);
			};
			evt.Failed += ex => {
				if (failedRaised)
					return;
				failedRaised = true;
				tcs.SetException (ex);
			};
			evt.Aborted += () => {
				if (abortedRaised)
					return;
				abortedRaised = true;
				tcs.SetException (new AbortedException ());
			};
			return tcs.Task;
		}



		public static Task<T> AsTask<T> (this IAwaitableEvent<T> evt)
		{
			return Await (evt);
		}



		public static Task Await (IAwaitableEvent evt)
		{
			var tcs = new TaskCompletionSource<object> ();

			var doneRaised = false;
			var failedRaised = false;
			var abortedRaised = false;

			evt.Done += () => {
				if (doneRaised)
					return;
				doneRaised = true;
				tcs.SetResult (null);
			};
			evt.Failed += ex => {
				if (failedRaised)
					return;
				failedRaised = true;
				tcs.SetException (ex);
			};
			evt.Aborted += () => {
				if (abortedRaised)
					return;
				abortedRaised = true;
				tcs.SetException (new AbortedException ());
			};
			return tcs.Task;
		}



		public static Task AsTask (this IAwaitableEvent evt)
		{
			return Await (evt);
		}
	}





	public class AbortedException : Exception
	{

	}
}

