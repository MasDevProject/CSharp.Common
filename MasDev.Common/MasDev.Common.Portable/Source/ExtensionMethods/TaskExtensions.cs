using System;
using System.Threading.Tasks;
using System.Threading;

namespace MasDev.Common.Extensions
{
	public class TaskResult
	{
		public Exception Exception { get; set; }

		public bool IsFaulted { get { return Exception != null; } }
	}

	public class TaskResult<T>
	{
		public T Result { get; set; }

		public Exception Exception { get; set; }

		public bool IsFaulted { get { return Exception != null; } }
	}

	public static class TaskExtensions
	{
		public static async Task<E> Concat<T,E> (this Task<T> task, Func<T, E> operation)
		{
			var result = await task;
			return operation (result);
		}

		public static bool IsFinishedSomeHow<T> (this Task<T> task)
		{
			return task.IsCanceled || task.IsCompleted || task.IsFaulted;
		}

		public static TaskResult<T> ToResult<T> (this Task<T> task)
		{
			if (!task.IsFinishedSomeHow ())
				throw new NotSupportedException ("Task must be finished in order to wrap it.");

			return new TaskResult<T> () {
				Exception = task.IsFaulted ? task.Exception : null,
				Result = task.IsFaulted ? default(T) : task.Result,
			};
		}

		public static void Then<T> (this Task<T> task, Action<TaskResult<T>> continuation)
		{
			task.GetAwaiter ().UnsafeOnCompleted (() => {
				continuation (task.ToResult ());
			});
		}

		public static void Then (this Task task, Action<TaskResult> continuation)
		{
			task.GetAwaiter ().UnsafeOnCompleted (() => {
				var result = new TaskResult () {
					Exception = task.IsFaulted ? task.Exception : null,
				};

				continuation (result);
			});
		}

		public static T WaitResult<T> (this Task<T> task, CancellationToken? ct = null)
		{
			if (ct == null)
				task.Wait ();
			else
				task.Wait ((CancellationToken)ct);

			var result = task.ToResult ();
			if (result.IsFaulted)
				throw result.Exception;

			return result.Result;
		}
	}
}

