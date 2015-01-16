using System;


namespace MasDev.Rest.Auth
{
	public interface IExpiration
	{
		TimeSpan GetExpiration (ICredentials credentials);
	}
}

