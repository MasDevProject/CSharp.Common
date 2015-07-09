using System;
using System.Threading.Tasks;

namespace MasDev.Services
{
	public abstract class ScheduledTask
	{
		readonly Func<IDisposable> _executionScopeProvider;

		protected ScheduledTask (Func<IDisposable> executionScopeProvider)
		{
			_executionScopeProvider = executionScopeProvider;
		}

		public void Run ()
		{
			SingleThreadSynchronizationContext.Run (RunInternal);
		}

		async Task RunInternal ()
		{
			using (var scope = _executionScopeProvider ()) {
				await DoJobAsync ();
			}
		}

		protected abstract Task DoJobAsync ();
	}
}

