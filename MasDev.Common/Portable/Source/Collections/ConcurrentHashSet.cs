using System.Collections.Generic;
using System.Threading;
using System;
using System.Linq;


namespace MasDev.Collections
{
	public class ConcurrentHashSet<T> : IDisposable
	{
		readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim (LockRecursionPolicy.SupportsRecursion);
		readonly HashSet<T> _hashSet = new HashSet<T> ();



		public bool Add (T item)
		{
			_lock.EnterWriteLock ();
			try
			{
				return _hashSet.Add (item);
			} finally
			{
				if (_lock.IsWriteLockHeld)
					_lock.ExitWriteLock ();
			}
		}



		public void Clear ()
		{
			_lock.EnterWriteLock ();
			try
			{
				_hashSet.Clear ();
			} finally
			{
				if (_lock.IsWriteLockHeld)
					_lock.ExitWriteLock ();
			}
		}



		public bool Contains (T item)
		{
			_lock.EnterReadLock ();
			try
			{
				return _hashSet.Contains (item);
			} finally
			{
				if (_lock.IsReadLockHeld)
					_lock.ExitReadLock ();
			}
		}



		public bool Remove (T item)
		{
			_lock.EnterWriteLock ();
			try
			{
				return _hashSet.Remove (item);
			} finally
			{
				if (_lock.IsWriteLockHeld)
					_lock.ExitWriteLock ();
			}
		}



		public int Count
		{
			get {
				_lock.EnterReadLock ();
				try
				{
					return _hashSet.Count;
				} finally
				{
					if (_lock.IsReadLockHeld)
						_lock.ExitReadLock ();
				}
			}
		}



		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}



		protected virtual void Dispose (bool disposing)
		{
			if (disposing)
			if (_lock != null)
				_lock.Dispose ();
		}



		~ConcurrentHashSet ()
		{
			Dispose (false);
		}



		public IQueryable<T> AsQueryable ()
		{
			return _hashSet.AsQueryable ();
		}
	}
}

