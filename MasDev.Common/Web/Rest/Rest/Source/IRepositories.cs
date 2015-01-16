
using System;
using MasDev.Rest.Auth;
using MasDev.Data;


namespace MasDev.Rest
{
	public interface IRepositories : IDisposable
	{
		IUnitOfWork SharedUnitOfWork { get; }

		ICredentialsRepository CredentialsRepository { get; }
	}





	public interface ICredentialsRepository
	{
		ICredentials Read (int id, int flag);



		void Update (ICredentials credentials);
	}
}

