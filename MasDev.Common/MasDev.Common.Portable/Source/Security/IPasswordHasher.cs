using System;

namespace MasDev.Common.Security
{
	public interface IPasswordHasher
	{
		bool IsPasswordValid (byte[] clearPassword, IHashedPassword password);

		IHashedPassword HashPassword (byte[] clearPassword);
	}
}

