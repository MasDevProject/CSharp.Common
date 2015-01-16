using System;
using System.Threading.Tasks;
using System.Threading;

namespace MasDev.Threading.Tasks
{
	public static class TaskLauncher
	{
		public static Task Periodicaly (TimeSpan interval, Action action, CancellationToken token)
		{
			return Task.Factory.StartNew (
				() => {
					for (;;) {
						if (token.WaitCancellationRequested (interval))
							break;

						action ();
					}
				}, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
		}

		public static Task PeriodicalyAsync (TimeSpan interval, Func<Task> asyncAction, CancellationToken token)
		{
			return Task.Factory.StartNew (
				async () => {
					for (;;) {
						if (token.WaitCancellationRequested (interval))
							break;

						await asyncAction ();
					}
				}, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
		}
	}
}

