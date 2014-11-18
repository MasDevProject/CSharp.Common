
using System;
using MasDev.Common.Rest.Auth;
using MasDev.Common.Data;


namespace MasDev.Common.Rest
{
	public interface IRepositories : IDisposable
	{
		IUnitOfWork SharedUnitOfWork { get; }

		ICredentialsRepository CredentialsRepository { get; }
	}





	public interface ICredentialsRepository
	{
		ICredentials Read (int id);



		void Update (ICredentials credentials);
	}
}

