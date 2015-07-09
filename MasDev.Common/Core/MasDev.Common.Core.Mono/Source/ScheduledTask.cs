using System;
using System.Threading.Tasks;
using MasDev.Threading;

namespace MasDev.Mono
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
			try {
				using (var scope = _executionScopeProvider ()) {
					try {
						await DoJobAsync ();
					} catch (Exception e) {
						OnException (e);
					}
				}
			} catch (Exception ex) {
				OnException (ex);
			}
		}

		protected virtual void OnException (Exception ex)
		{
			throw ex;
		}

		protected abstract Task DoJobAsync ();
	}
}

