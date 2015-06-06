using System;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;


namespace MasDev.Utils
{
	public class LockByIdMutex : IDisposable
	{
		readonly bool _hasHandle;
		readonly Mutex _mutex;

		public LockByIdMutex (int id) : this (id.ToString ())
		{

		}


		public LockByIdMutex (string mutexId)
		{
			bool createdNew;

			var allowEveryoneRule = new MutexAccessRule (new SecurityIdentifier (WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
			var securitySettings = new MutexSecurity ();
			securitySettings.AddAccessRule (allowEveryoneRule);

			_mutex = new Mutex (false, mutexId, out createdNew, securitySettings);
			try {
				_hasHandle = _mutex.WaitOne (5000, false);
				if (!_hasHandle)
					throw new TimeoutException ("Timeout waiting for exclusive access");
			} catch (AbandonedMutexException) {
				_hasHandle = true;
			}
		}



		public void Dispose ()
		{
			if (_hasHandle)
				_mutex.ReleaseMutex ();
			_mutex.Dispose ();
		}

	}
}

