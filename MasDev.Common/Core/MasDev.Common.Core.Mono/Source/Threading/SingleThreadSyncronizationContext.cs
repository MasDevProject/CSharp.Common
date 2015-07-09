using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MasDev.Threading
{
	public sealed class SingleThreadSynchronizationContext : SynchronizationContext
	{
		int threadId;

		struct Job
		{
			public Job (SendOrPostCallback action, object state, TaskCompletionSource<bool> onCompleted)
			{
				this.Action = action;
				this.State = state;
				this.OnCompleted = onCompleted;
			}

			public readonly SendOrPostCallback Action;
			public readonly object State;
			public readonly TaskCompletionSource<bool> OnCompleted;
		}

		private BlockingCollection<Job> m_queue = new BlockingCollection<Job> ();
		private LateArrivalBehavior lateArrivalBehavior;
		private SingleThreadSynchronizationContext onFailureForwardTo;
		private int firstLateArrivalCallbackExecuted;
		private TaskCompletionSource<bool> drainingCompletion = new TaskCompletionSource<bool> ();

		public override void Post (SendOrPostCallback d, object state)
		{
			Interlocked.Increment (ref CtxSwitchCount);

			EnqueueJob (new Job (d, state, null), true);
		}

		private bool EnqueueJob (Job job, bool allowForward)
		{

			if (!completed) {
				try {
					var q = m_queue;
					q.Add (job);
					return true;
				} catch (InvalidOperationException) {
				}
			}

			if (onFirstLateArrival != null) {
				if (Interlocked.Increment (ref firstLateArrivalCallbackExecuted) == 1) {
					onFirstLateArrival ();
					onFirstLateArrival = null;
				}
			}

			if (allowForward && onFailureForwardTo != null)
				return onFailureForwardTo.EnqueueJob (job, false);

			if (lateArrivalBehavior == LateArrivalBehavior.Throw) {
				var frames = new System.Diagnostics.StackTrace (false).GetFrames ();
				foreach (var frame in frames) {
					var m = frame.GetMethod ();
					if (m != null) {
						if (m.DeclaringType == typeof(SingleThreadSynchronizationContext))
							continue;
						if (m.DeclaringType.Assembly == typeof(Task).Assembly && m.DeclaringType.Name == "SynchronizationContextAwaitTaskContinuation")
							return false; // We have to pretend nothing happened and not to throw the exception
						break;
					}
				}
				throw new InvalidOperationException ("The SingleThreadSynchronizationContext has completed and is no longer accepting continuations or callbacks.");
			}
			if (lateArrivalBehavior == LateArrivalBehavior.SpawnNewThread) {
				lock (this) {
					if (lateArrivalSyncCtx == null || !lateArrivalSyncCtx.EnqueueJob (job, false)) {
						var readyTcs = new TaskCompletionSource<bool> ();
						var firstContinuationExecutedTcs = new TaskCompletionSource<bool> ();
						Action deleg = () => {
							SingleThreadSynchronizationContext.Run (async () => {
								var ctx = (SingleThreadSynchronizationContext)SynchronizationContext.Current;
								ctx.onFailureForwardTo = this;
								lateArrivalSyncCtx = ctx;
								readyTcs.SetResult (true);
								drainingCompletion.Task.Wait ();
								await firstContinuationExecutedTcs.Task;
								await Task.Delay (10000);
							}, LateArrivalBehavior.Suppress);
						};
						#if DESKTOP
						new Thread(() => deleg()) { Name = "Respawned thread for SingleThreadSynchronizationContext" }.Start();
						#else
						Task.Run (deleg);
						#endif
						readyTcs.Task.Wait ();
						if (!lateArrivalSyncCtx.EnqueueJob (job, false))
							throw new Exception ();
						firstContinuationExecutedTcs.SetResult (true);
					}
					return true;
				}
			}


			return false;

		}

		private SingleThreadSynchronizationContext lateArrivalSyncCtx;
		private bool completed;
		private Action onFirstLateArrival;

		public bool HasPendingContinuations {
			get {
				return this.m_queue.Count != 0;
			}
		}

		public override void Send (SendOrPostCallback d, object state)
		{
			if (Environment.CurrentManagedThreadId == threadId) {
				d (state);
			} else {
				var tcs = new TaskCompletionSource<bool> ();
				if (!EnqueueJob (new Job (d, state, tcs), true))
					throw new InvalidOperationException ("The target SynchronizationContext has completed and is no longer accepting tasks.");
				Interlocked.Increment (ref CtxSwitchCount);
				tcs.Task.Wait ();
			}
		}

		public static long CtxSwitchCount;

		private void RunOnCurrentThread ()
		{
			while (true) {
				Job workItem;
				try {
					var queue = m_queue;
					if (queue == null)
						return;
					if (!queue.TryTake (out workItem, Timeout.Infinite, CancellationToken.None))
						return;
				} catch (OperationCanceledException) {
					return;
				}

				bool faulted = false;
				try {
					workItem.Action (workItem.State);
				} catch (Exception ex) {
					if (workItem.OnCompleted != null)
						workItem.OnCompleted.SetException (ex);
					faulted = true;
				}

				if (!faulted && workItem.OnCompleted != null)
					workItem.OnCompleted.SetResult (true);
			}
		}

		private SingleThreadSynchronizationContext ()
		{

			threadId = Environment.CurrentManagedThreadId;

		}


		//~SingleThreadSynchronizationContext()
		//{
		//    Console.WriteLine("Finalizing " + this.GetHashCode() + " ***************************");
		//}

		public static SingleThreadSynchronizationContext CreateInNewThread ()
		{
			return CreateInNewThread (LateArrivalBehavior.Throw);
		}

		public static SingleThreadSynchronizationContext CreateInNewThread (LateArrivalBehavior lateArrivalBehavior)
		{
			return CreateInNewThread (lateArrivalBehavior, null);
		}

		public static SingleThreadSynchronizationContext CreateInNewThread (LateArrivalBehavior lateArrivalBehavior, string threadName)
		{
			var tcs = new TaskCompletionSource<SingleThreadSynchronizationContext> ();
			var thread = new Thread (() => {
				try {
					Run (() => {
						tcs.TrySetResult ((SingleThreadSynchronizationContext)SynchronizationContext.Current);
						return new TaskCompletionSource<bool> ().Task;
					}, lateArrivalBehavior);
				} catch (Exception ex) {
					tcs.TrySetException (ex);
				}

			});
			if (threadName != null)
				thread.Name = threadName;
			thread.IsBackground = true;
			thread.SetApartmentState (ApartmentState.STA);
			thread.IsBackground = true;
			thread.Start ();
			tcs.Task.Wait ();
			return tcs.Task.Result;
		}

		public static async Task RunAsync (Func<Task> func)
		{
			await Task.Run (() => Run (func, LateArrivalBehavior.Throw, null));
		}

		public static void Run (Func<Task> func)
		{
			Run (func, LateArrivalBehavior.Throw, null);
		}

		public static void Run (Func<Task> func, LateArrivalBehavior lateArrivalBehavior)
		{
			Run (func, lateArrivalBehavior, null);
		}


		public static void Run (Func<Task> func, Action onFirstLateArrival)
		{
			Run (func, LateArrivalBehavior.Suppress, onFirstLateArrival);
		}

		private static void Run (Func<Task> func, LateArrivalBehavior lateArrivalBehavior, Action onFirstLateArrival)
		{
			if (func == null)
				throw new ArgumentNullException ("func");

			var prevCtx = SynchronizationContext.Current;

			var syncCtx = new SingleThreadSynchronizationContext ();
			try {
				syncCtx.onFirstLateArrival = onFirstLateArrival;
				//Console.WriteLine("Running " + syncCtx.GetHashCode() + " on thread " + Environment.CurrentManagedThreadId);
				syncCtx.lateArrivalBehavior = lateArrivalBehavior;
				SynchronizationContext.SetSynchronizationContext (syncCtx);

				var t = func ();
				if (t == null)
					throw new InvalidOperationException ("No task provided.");
				t.ContinueWith (delegate {
					try {
						var q = syncCtx.m_queue;
						if (q != null) {
							q.CompleteAdding ();
						}

					} catch {
					}
					syncCtx.completed = true;
				}, TaskScheduler.Default);
				syncCtx.RunOnCurrentThread ();
				if (!syncCtx.aborted) {
					t.GetAwaiter ().GetResult ();
				}
				if (syncCtx.aborted)
					throw new OperationCanceledException ("The SingleThreadSynchronizationContext was aborted.");
			} finally {
				SynchronizationContext.SetSynchronizationContext (prevCtx);

				var q = syncCtx.m_queue;
				if (q != null)
					q.Dispose ();
				syncCtx.m_queue = null;

				var d = syncCtx.drainingCompletion;
				if (d != null)
					d.TrySetResult (true);

			}
		}

		volatile bool aborted;

		public enum LateArrivalBehavior
		{
			Throw,
			Suppress,
			SpawnNewThread
		}

		public void Abort ()
		{
			completed = true;
			aborted = true;
			var q = this.m_queue;
			if (q != null) {

				try {
					q.CompleteAdding ();
				} catch (ObjectDisposedException) {
				}
				if (q.Count == 0) {
					q.Dispose ();
					this.m_queue = null;
				}
				var f = this.onFailureForwardTo;
				if (f != null)
					f.Abort ();
				this.onFailureForwardTo = null;
				f = this.lateArrivalSyncCtx;
				if (f != null)
					f.Abort ();
				this.lateArrivalSyncCtx = null;
				this.onFirstLateArrival = null;
			}
		}


	}
}