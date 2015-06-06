using System;

namespace MasDev.Common
{
	public interface IExclusiveAccessProvider
	{
		IDisposable StartExclusiveAccess (IIdentifier identifier);
	}

	public interface IIdentifier
	{
		string Identify { get; }
	}
}