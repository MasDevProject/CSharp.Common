using NHibernate;
using System.Data;
using System;


namespace MasDev.Data
{
	public class NHibernateUnitOfWork : IUnitOfWork
	{
		readonly ISession _session;
		readonly Lazy<ISession> _readonlySession;

		ITransaction _transaction;
		volatile int _consumers = 0;

		public NHibernateUnitOfWork (ISessionFactory factory)
		{
			if (factory == null)
				throw new ArgumentNullException ();
			_session = factory.OpenSession ();
			_session.FlushMode = FlushMode.Commit;
		
			_readonlySession = new Lazy<ISession> (() => {
				var session = factory.OpenSession ();
				session.FlushMode = FlushMode.Never;
				return session;
			}, true);
		}

		public void Consume ()
		{
			_consumers++;
		}

		public ISession Session { get { return _session; } }

		public ISession ReadonlySession { get { return _readonlySession.Value; } }

		public void Start ()
		{
			_transaction = _session.BeginTransaction (IsolationLevel.ReadCommitted);
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
			if (--_consumers != 0)
				return;

			if (_transaction != null)
				_transaction.Dispose ();

			if (_readonlySession.IsValueCreated)
				_readonlySession.Value.Dispose ();

			_session.Close ();
			_session.Dispose ();
		}
	}
}

