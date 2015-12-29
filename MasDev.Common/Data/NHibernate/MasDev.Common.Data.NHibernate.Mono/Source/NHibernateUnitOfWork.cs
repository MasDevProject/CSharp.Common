using NHibernate;
using System.Data;
using System;
using System.Diagnostics;


namespace MasDev.Data
{
	public interface INHibernateUnitOfWork : IUnitOfWork
	{
		ISession Session { get ; }

		ISession ReadonlySession { get; }

		ISession ParallelSession { get; }

		void Start ();
	}

	public class NHibernateUnitOfWork : INHibernateUnitOfWork
	{
		readonly ISessionFactory _factory;
		readonly ISession _session;
		readonly Lazy<ISession> _readonlySession;
		readonly Lazy<ISession> _parallelSession;

		ITransaction _transaction;

		public NHibernateUnitOfWork (ISessionFactory factory)
		{
			if (factory == null)
				throw new ArgumentNullException ();
			
			Debug.WriteLine ("Uow constructed");

			_factory = factory;
			_session = GetSession (factory, FlushMode.Commit);
			_parallelSession = new Lazy<ISession> (() => GetSession (_factory, FlushMode.Commit));
			_readonlySession = new Lazy<ISession> (() => GetSession (_factory, FlushMode.Never));
		}

		static ISession GetSession (ISessionFactory factory, FlushMode flushMode)
		{
			var session = factory.OpenSession ();
			session.FlushMode = flushMode;
			return session;
		}

		public ISession Session { get { return _session; } }

		public ISession ReadonlySession { get { return _readonlySession.Value; } }

		public ISession ParallelSession { get { return _parallelSession.Value; } }

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
			Debug.WriteLine ("Uow closed");

			if (_transaction != null)
				_transaction.Dispose ();

			_session.Dispose ();
			DisposeSession (_parallelSession);
			DisposeSession (_readonlySession);
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

