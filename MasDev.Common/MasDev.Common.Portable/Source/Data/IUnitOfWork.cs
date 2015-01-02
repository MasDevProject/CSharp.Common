using System;


namespace MasDev.Data
{
	public interface IUnitOfWork : IDisposable
	{
		void Commit ();



		void Commit (bool startNew);



		void Rollback ();



		void Rollback (bool startNew);



		bool IsStarted { get; }
	}
}

