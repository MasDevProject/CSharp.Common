using NHibernate;
using System.Data;
using System;
using System.Diagnostics;


namespace MasDev.Data
{
	public class NHibernateUnitOfWork : IUnitOfWork
	{
		readonly Lazy<ISession> _session;
		readonly Lazy<ISession> _readonlySession;
		readonly Lazy<ISession> _parallelSession;

		ITransaction _transaction;

		public NHibernateUnitOfWork (ISessionFactory factory)
		{
			if (factory == null)
				throw new ArgumentNullException ();
			
			Debug.WriteLine ("Uow constructed");

			_session = GetDefaultSessionLazy (factory);
			_parallelSession = GetDefaultSessionLazy (factory);

			_readonlySession = new Lazy<ISession> (() => {
				var session = factory.OpenSession ();
				session.FlushMode = FlushMode.Never;
				return session;
			}, true);
		}

		static Lazy<ISession> GetDefaultSessionLazy (ISessionFactory factory)
		{
			return new Lazy<ISession> (() => {
				var session = factory.OpenSession ();
				session.FlushMode = FlushMode.Commit;
				return session;
			});
		}

		public ISession Session { get { return _session.Value; } }

		public ISession ReadonlySession { get { return _readonlySession.Value; } }

		public ISession ParallelSession { get { return _parallelSession.Value; } }

		public void Start ()
		{
			_transaction = _session.Value.BeginTransaction (IsolationLevel.ReadCommitted);
		}

		public bool IsStarted { get { return _transaction != null; } }

		public void Rollback ()
		{
			Rollback (true);
		}

		public void Rollback (bool startNew)
		{

			if (_transaction != null) {
				if (_transaction.IsActive && !_transaction.WasRolledBack)
					_transaction.Rollback ();

				_transaction.Dispose ();
				_transaction = null;
			}

			if (startNew)
				Start ();
		}

		public void Commit ()
		{
			Commit (true);
		}

		public void Commit (bool startNew)
		{
			if (_transaction != null) {
				if (_transaction.IsActive && !_transaction.WasRolledBack)
					_transaction.Commit ();
					
				_transaction.Dispose ();
				_transaction = null;
			}

			if (startNew)
				Start ();
		}

		public void Dispose ()
		{
			Debug.WriteLine ("Uow disposed");

			if (_transaction != null)
				_transaction.Dispose ();

			if (_readonlySession.IsValueCreated)
				_readonlySession.Value.Dispose ();

			DisposeSession (_parallelSession);
			DisposeSession (_session);
		}

		static void DisposeSession (Lazy<ISession> session)
		{
			if (!session.IsValueCreated)
				return;
			session.Value.Close ();
			session.Value.Dispose ();
		}
	}
}

