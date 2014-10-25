
using System;
using MasDev.Common.Rest.Auth;


namespace MasDev.Common.Rest
{
	public interface IRepositories : IDisposable
	{
		ICredentialsRepository CredentialsRepository { get; }
	}





	public interface ICredentialsRepository
	{
		ICredentials Read (int id);



		void Update (ICredentials credentials);
	}
}

