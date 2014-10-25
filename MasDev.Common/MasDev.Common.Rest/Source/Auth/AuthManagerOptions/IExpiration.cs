using System;


namespace MasDev.Common.Rest.Auth
{
	public interface IExpiration
	{
		TimeSpan GetExpiration (ICredentials credentials);
	}
}

