using System;


namespace MasDev.Data
{
    public interface IUnitOfWork
    {
        void Commit();

        void Commit(bool startNew);

        void Rollback();

        void Rollback(bool startNew);

        bool IsStarted { get; }

        void Close();
    }
}

